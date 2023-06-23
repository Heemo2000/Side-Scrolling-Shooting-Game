using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    private int _prefabID = -1;

    public int PrefabID { get => _prefabID; set => _prefabID = value; }

    public virtual void Reuse()
    {
        gameObject.SetActive(true);
    }

    protected virtual void Destroy()
    {
        if(_prefabID != -1)
        {
            this.gameObject.SetActive(false);
            ObjectPoolManager.Instance.ReturnObjectToPool(this);
            return;
        }
        Destroy(gameObject);
    }
}
