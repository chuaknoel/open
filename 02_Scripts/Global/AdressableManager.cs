using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour
{
    private Dictionary<string, AsyncOperationHandle> loadedAssets = new(); // 로드된 에셋 주소와 핸들을 저장하는 곳

    public static AddressableManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// 지정된 Addressables 주소의 에셋을 비동기로 로드하고, 로드가 완료되면 제네릭 타입으로 반환합니다.
    /// </summary>
    /// <summary>
    /// 지정된 주소의 에셋을 비동기로 로드하고, 전달받은 컨테이너(List 또는 Dictionary)에 저장합니다.
    /// </summary>
    public async Task<T> LoadAsset<T>(string address) where T : Object
    {
        if (loadedAssets.ContainsKey(address))
        {
            var matchHandle = loadedAssets[address];
            return matchHandle.Result as T;
        }

        var locationHandle = Addressables.LoadResourceLocationsAsync(address);
        await locationHandle.Task;

        if (locationHandle.Status != AsyncOperationStatus.Succeeded || locationHandle.Result.Count == 0)
        {
            Logger.Log($"Addressable asset not found at address: {address}");
            return null;
        }

        var handle = Addressables.LoadAssetAsync<T>(address);
        loadedAssets[address] = handle;

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Logger.Log($"[AddressableManager] Success to preload {address}");
            return handle.Result;
        }
        else
        {
            Logger.Log($"[AddressableManager] Failed to preload {address}");
            return null;
        }
    }


    /// <summary>
    /// 특정 주소의 에셋 메모리에서 해제
    /// </summary>
    public void Release(string address)
    {
        if (loadedAssets.TryGetValue(address, out var handle))
        {
            Addressables.Release(handle);
            loadedAssets.Remove(address);
        }
    }
    /// <summary>
    /// 모든 로드된 에셋을 메모리에서 해제
    /// </summary>
    public void ReleaseAll()
    {
        foreach (var handle in loadedAssets.Values)
        {
            Addressables.Release(handle);
        }
        loadedAssets.Clear();
    }
}