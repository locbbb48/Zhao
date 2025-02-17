/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : Component
{
    public static ObjectPool<T> Instance { get; set; }

    public T prefab;
    public int initialPoolSize = 16;

    public Transform ActiveParent; // Parent của object SetActive(true)
    public Transform PoolParent;   // Parent của object SetActive(false)

    public Queue<T> objects;

    public virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        objects = new Queue<T>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            T obj = Instantiate(prefab);
            obj.transform.SetParent(PoolParent);
            obj.gameObject.SetActive(false);
            objects.Enqueue(obj);
        }
    }

    public T GetObject()
    {
        if (objects.Count == 0)
        {
            T obj = Instantiate(prefab);
            obj.transform.SetParent(PoolParent);
            obj.gameObject.SetActive(false);
            objects.Enqueue(obj);
        }

        T objToReturn = objects.Dequeue();
        objToReturn.transform.SetParent(ActiveParent);
        return objToReturn;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(PoolParent);
        objects.Enqueue(obj);
    }
}