using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    private Dictionary<GameObject, Queue<GameObject>> pool = new();

    void Awake()
    {

        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

    }

    public GameObject GetObject(GameObject prefab)
    {
        if (!pool.ContainsKey(prefab))
            pool[prefab] = new Queue<GameObject>();

        if (pool[prefab].Count > 0)
        {
            GameObject obj = pool[prefab].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(prefab);
    }

    public void ReturnObject(GameObject prefab, GameObject obj)
    {
        if (!pool.ContainsKey(prefab))
            pool[prefab] = new Queue<GameObject>();

        obj.SetActive(false);
        pool[prefab].Enqueue(obj);
    }
}