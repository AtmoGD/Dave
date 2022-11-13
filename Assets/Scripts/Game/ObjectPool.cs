using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ObjectPoolData
{
    [SerializeField] public GameObject prefab = null;
    [SerializeField] public int poolSize = 0;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; protected set; }
    [SerializeField] private List<ObjectPoolData> objectsToPool = new List<ObjectPoolData>();
    private List<GameObject> pooledObjects = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadPool();
    }

    private void LoadPool()
    {
        foreach (ObjectPoolData objectToPool in objectsToPool)
        {
            for (int i = 0; i < objectToPool.poolSize; i++)
            {
                GameObject obj = Instantiate(objectToPool.prefab, transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    // public T Get<T>()
    // {
    //     GameObject obj = pooledObjects.Find(o => o.GetComponent<T>() != null && !o.activeSelf);
    //     if (obj == null)
    //     {
    //         ObjectPoolData objectToPool = objectsToPool.Find(o => o.prefab.GetComponent<T>() != null);
    //         if (objectToPool == null)
    //         {
    //             Debug.LogError("ObjectPool: Object of type " + typeof(T).ToString() + " not found in pool");
    //             return default;
    //         }
    //         obj = Instantiate(objectToPool.prefab, transform);
    //         pooledObjects.Add(obj);
    //     }
    //     obj.SetActive(true);
    //     return obj.GetComponent<T>();
    // }

    public GameObject Get(GameObject _prefab)
    {
        GameObject obj = pooledObjects.Find(o => o.name == (_prefab.name + "(Clone)") && !o.activeSelf);
        if (obj == null)
        {
            obj = Instantiate(_prefab, transform);
            pooledObjects.Add(obj);
        }
        obj.SetActive(true);
        return obj;
    }
}
