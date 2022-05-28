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
        public string tag;
        public GameObject prefab;
        public int size;
    }
    //list of pool configurations to show on the inspector
    [SerializeField]
    private List<Pool> pools;

    //dictionary of pools with pool tag and queue of game objects
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    //build the configured pools at the start
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            //instantiate the specified number of objects as inactive an enqueue for spawn
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
        }
    }
    private GameObject Spawn(string tag, Vector3 position, Quaternion rotation)
    {
        //get object from the pool
        GameObject gameObject = poolDictionary[tag].Dequeue();

        //activate and set the specified transform
        gameObject.SetActive(true);
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        //put it at the back of the queue
        poolDictionary[tag].Enqueue(gameObject);

        return gameObject;
    }
}
