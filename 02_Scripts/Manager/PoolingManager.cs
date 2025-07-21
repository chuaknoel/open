using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance { get; private set; }

    public Dictionary<string, object> poolingDictionary = new Dictionary<string, object>();

    public Transform projectilePool;

    private void Awake()
    {
        if (Instance != null)
        {
            Logger.LogError($"[YourManager] Duplicate instance detected on '{gameObject.name}'. " +
                        $"This may indicate a missing unload or unintended duplicate. Destroying this instance.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public IObjectPool<T> CreatePool<T>(string address, Func<T> create, int _maxSize = 1) where T : MonoBehaviour
    {
        RegisterPoolObject(address,

           new ObjectPool<T>
           (
               create,           //발사체 오브젝트 생성 로직
               OnGet,            //생성된 오브젝트를 소환
               OnRelease,        //생성된 오브젝트 회수
               OnDes,            //생성된 오브젝트 파괴
               maxSize: _maxSize //한번에 관리될 오브젝트 갯수
           ));

        IObjectPool<T> pool = FindPool<T>(address);

        return pool;
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

    public void UnLoad()
    {
        if (Instance == this)
        {
            poolingDictionary.Clear();
            Instance = null;
        }
        else if (Instance == null)
        {
            Logger.LogError($"[YourManager] UnLoad called, but Instance was already null. Possible duplicate unload or uninitialized state.");
        }
        else
        {
            Logger.LogError($"[YourManager] UnLoad called by a non-instance object: {gameObject.name}. Current Instance is on {Instance.gameObject.name}");
        }
    }
}
