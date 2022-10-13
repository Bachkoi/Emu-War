using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPooler : MonoBehaviour
{
    #region Classes
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    #endregion

    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Fields
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        //Fills dicitonary with pool references.
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    //Gets reference to the pool to pull from and set it active
    public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion quat)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Tag " + tag + " does not exist");
            return null;
        }
        GameObject objectSpawned = poolDictionary[tag].Dequeue();
        objectSpawned.SetActive(true);
        objectSpawned.transform.position = pos;
        objectSpawned.transform.rotation = quat;

        poolDictionary[tag].Enqueue(objectSpawned);
        return objectSpawned;
    }

}