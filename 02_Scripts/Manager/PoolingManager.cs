using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance;

    public Dictionary<string, object> poolingDictionary = new Dictionary<string, object>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterPoolObject<T>(string poolName, IObjectPool<T> objectPrefab) where T : class
    {
        if (poolingDictionary.ContainsKey(poolName)) return;

        poolingDictionary.Add(poolName , objectPrefab);
    }

    //외부에서 오브젝트 풀을 사용할때 오브젝트 이름으로부터 해당 오브젝트 풀을 찾아준다.
    public IObjectPool<T> FindPool<T>(string _foolName) where T : class
    {
        if (poolingDictionary.TryGetValue(_foolName, out var projectile))
        {
            return projectile as IObjectPool<T>;
        }

        return null;
    }

    //오브젝트 생성
    public void OnGet<T>(T _poolObj) where T : MonoBehaviour
    {
        _poolObj.gameObject.SetActive(true);
    }

    //오브젝트 회수
    public void OnRelease<T>(T _poolObj) where T : MonoBehaviour
    {
        _poolObj.gameObject.SetActive(false);
    }

    //오브젝트 파괴
    public void OnDes<T>(T _poolObj) where T : MonoBehaviour
    {
        Destroy(_poolObj.gameObject);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
