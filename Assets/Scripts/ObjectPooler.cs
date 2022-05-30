using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //singleton
    public static ObjectPooler Instance;
    private void Awake() => Instance = this;

    //class for pool configurations. serializable to be able to use it on the inspector
    [System.Serializable]
    public class Pool
    {
        public ObjectType type;
        public GameObject prefab;
        public int size;
    }
    //list of pool configurations to show on the inspector
    [SerializeField]
    private List<Pool> pools;

    //dictionary of pools with pool tag and queue of game objects
    public Dictionary<ObjectType, Queue<GameObject>> poolDictionary;

    //build the configured pools at the start
    private void Start()
    {
        poolDictionary = new Dictionary<ObjectType, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            //instantiate the specified number of objects as inactive an enqueue for spawn
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.type, objectPool);
        }
    }
    public GameObject Spawn(ObjectType type, Vector3 position, Quaternion rotation)
    {
        //get object from the pool
        GameObject gameObject = poolDictionary[type].Dequeue();

        //activate and set the specified transform
        gameObject.SetActive(true);
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        //put it at the back of the queue
        poolDictionary[type].Enqueue(gameObject);

        return gameObject;
    }
    public void Add(GameObject obj, ObjectType type)
    {
        poolDictionary[type].Enqueue(obj);
    }
    public void Kill(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = transform;
    }

    public enum ObjectType { Money, Gold, Diamond }
}
