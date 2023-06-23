using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolObjectData
{
    public PoolObject prefab;
    public int count;
}
public class ObjectPoolManager : GenericSingleton<ObjectPoolManager>
{
    [SerializeField]private PoolObjectData[] prefabData;
    private Dictionary<int,Queue<PoolObject>> _poolDictionary;
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        _poolDictionary = new Dictionary<int, Queue<PoolObject>>();
    }

    private void Start() 
    {
        foreach(PoolObjectData poolObjectData in prefabData)
        {
            ObjectPoolManager.Instance.CreatePool(poolObjectData.prefab.gameObject,poolObjectData.count);
        }
    }

    public void CreatePool(GameObject prefab,int poolSize)
    {
        int id = prefab.GetInstanceID();
        string poolName = prefab.name + " pool";
        if(_poolDictionary.ContainsKey(id) == false)
        {
            _poolDictionary.Add(id,new Queue<PoolObject>());
            GameObject poolObjectsContainer = new GameObject(poolName);
            poolObjectsContainer.transform.parent = transform;
            
            for(int i = 0; i < poolSize; i++)
            {
                GameObject instance = Instantiate(prefab,Vector3.zero,Quaternion.identity);
                PoolObject poolObjectComp = instance.GetComponent<PoolObject>();
                instance.transform.parent = poolObjectsContainer.transform;
                poolObjectComp.PrefabID = id;
                instance.SetActive(false);
                _poolDictionary[id].Enqueue(poolObjectComp);
                
            }
        }

        Debug.LogError(poolName + " already exists!");
        
    }

    public PoolObject ReuseObject(GameObject prefab,Vector3 position,Quaternion rotation)
    {
        int id = prefab.GetInstanceID();

        if(!_poolDictionary.ContainsKey(id))
        {
            Debug.LogError(prefab.name + " Pool not found!!");
            return null;
        }

        PoolObject poolObject = null;
        if(_poolDictionary[id].Count > 0)
        {
            poolObject = _poolDictionary[id].Dequeue();
            poolObject.transform.position = position;
            poolObject.transform.rotation = rotation;
        }
        else
        {
            GameObject instance = Instantiate(prefab, position, rotation);
            poolObject = instance.GetComponent<PoolObject>();
        }

        poolObject.Reuse();
        return poolObject;
    }

    public void ReturnObjectToPool(PoolObject poolObject)
    {
        if(poolObject == null)
        {
            Debug.LogError("Null object cannot be returned to pool!!");
            return;
        }

        
        if(!_poolDictionary.ContainsKey(poolObject.PrefabID))
        {
            Debug.LogError(poolObject.name + " Pool not found!!");
            return;
        }

        
        _poolDictionary[poolObject.PrefabID].Enqueue(poolObject);

    }
}
