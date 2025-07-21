using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

using System.Threading.Tasks;

public class TestEnemy : MonoBehaviour
{
    //public static bool isBattleScene;

    public Scene defalutScene;
    public Scene battleScene;

    public string DefaultScene;
    public string BattleScene;

    public List<GameObject> defalutObject;
    public List<GameObject> battleObject;

    private void Awake()
    {
        defalutScene = SceneManager.GetSceneByName(DefaultScene);
        battleScene = SceneManager.GetSceneByName(BattleScene);

        DefaultSceneToggle(true);
        BattleSceneToggle(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            EnterBattle();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ExitBattle();
        }
    }

    public void EnterBattle()
    {
        StartCoroutine(EnterBattleRoutine());
    }

    IEnumerator EnterBattleRoutine()
    {
        Time.timeScale = 0;
        DefaultSceneToggle(false);
        BattleSceneToggle(true);
        yield return null;

        Time.timeScale = 1;
    }

    /// <summary>
    /// 전투 종료 (베틀씬 Unload → 탐험씬 Additive Load)
    /// </summary>
    public void ExitBattle()
    {
        StartCoroutine(ExitBattleRoutine());
    }

    IEnumerator ExitBattleRoutine()
    {
        Time.timeScale = 0;
        DefaultSceneToggle(true);
        BattleSceneToggle(false);
        yield return null;

        //isBattleScene = false;
        //GameManager.Instance.StartScene(defaultScene, LoadSceneMode.Single);

        yield return null;

        Time.timeScale = 1;

        Logger.Log("Default Scene Loaded & Active!");
    }

    public void DefaultSceneToggle(bool isOpen)
    {
        for (int i = 0; i < defalutObject.Count; i++) 
        {
            defalutObject[i].SetActive(isOpen);
        }
    }

    public void BattleSceneToggle(bool isOpen)
    {
        for (int i = 0; i < battleObject.Count; i++)
        {
            battleObject[i].SetActive(isOpen);
        }
    }


    private Dictionary<string, AsyncOperationHandle> loadedAssets = new(); // 로드된 에셋 주소와 핸들을 저장하는 곳

    public async void Test()
    {
        var asset = await Load<GameObject>("string");
    }

    public async Task<T> Load<T>(string address) where T : Object
    {
        if (!loadedAssets.ContainsKey(address))
        {
            var locationHandle = Addressables.LoadResourceLocationsAsync(address);
            await locationHandle.Task;

            if (locationHandle.Status == AsyncOperationStatus.Succeeded && locationHandle.Result.Count > 0)
            {
                var location = locationHandle.Result[0];
                var handle = Addressables.LoadAssetAsync<T>(location);
                await handle.Task;

                loadedAssets[address] = handle;
                Logger.Log($"[AddressableManager] Success to preload {address}");
                return handle.Result;
            }
            else
            {
                Logger.Log($"[AddressableManager] Failed to preload {address}");
                return null;
            }
        }

        return loadedAssets[address].Result as T;
    }

    public T LoadAsset<T>(string address) where T : Object
    {
        StartCoroutine(LoadAndGetAsset<T>(address));

        return loadedAssets[address].Result as T;
    }

    public IEnumerator LoadAndGetAsset<T>(string address) where T : Object
    {
        if (!loadedAssets.ContainsKey(address))
        {
            var locationHandle = Addressables.LoadResourceLocationsAsync(address);
            yield return locationHandle;

            if (locationHandle.Status == AsyncOperationStatus.Succeeded && locationHandle.Result.Count > 0)
            {
                var location = locationHandle.Result[0];
                var handle = Addressables.LoadAssetAsync<Object>(location);
                yield return handle;

                loadedAssets[address] = handle;
                Logger.Log($"[AddressableManager] Success to preload {address}");
            }
            else
            {
                Logger.Log($"[AddressableManager] Failed to preload {address}");
                yield break;
            }
        }
    }
}
