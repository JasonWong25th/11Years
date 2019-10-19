using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public string tag;
    public GameObject prefab;
    public int amountToPool;
    public ObjectPoolItem(GameObject obj, int amt)
    {
        prefab = obj;
        amountToPool = Mathf.Max(amt, 2);
    }
}

public class ObjectPooler : MonoBehaviour
{
    private static ObjectPooler SharedInstance;

    public static ObjectPooler Instance
    {
        get { return SharedInstance; }
    }

    public void Awake()
    {
        SharedInstance = this;
    }

    public List<ObjectPoolItem> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (ObjectPoolItem pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.amountToPool; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Vector3 randVector)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Poo with tag" + tag + "Does not exist");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);


        //To randomize thier poistion and rotation

        Vector3 randPosition = new Vector3(Random.Range(position.x - randVector.x, position.x + randVector.x), Random.Range(position.y - randVector.y, position.y + randVector.y), Random.Range(position.z - randVector.z, position.z + randVector.z));

        objectToSpawn.transform.position = randPosition;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnSpawnObject();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}