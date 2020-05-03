using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    static ObjectPoolManager _instance;

    public static ObjectPoolManager instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<ObjectPoolManager>();
            }
            return _instance;
        }
    }

    //Used to store and find the specific object pool for a specific object.
    //TODO: Consider using string for type of pool instead.
    Dictionary<int, Queue<GameObject>> objectPoolDictionary = new Dictionary<int, Queue<GameObject>>();

    public void CreateNewObjectPool(GameObject prefab, int poolSize)
    {
        int objectPoolKey = prefab.GetInstanceID();

        //If that type of pool doesnt already exist, create it.
        if(!objectPoolDictionary.ContainsKey(objectPoolKey))
        {
            objectPoolDictionary.Add(objectPoolKey, new Queue<GameObject>());

            GameObject poolHolder = new GameObject(prefab.name + " pool");
            poolHolder.transform.parent = transform;

            //Setup the first number of objects in that pool.
            for(int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab) as GameObject;
                newObject.SetActive(false);
                newObject.transform.SetParent(poolHolder.transform);
                objectPoolDictionary[objectPoolKey].Enqueue(newObject);
            }
        }
    }

    //Used to add the object back to its pool, meaning it is ready to be reused.
    public void DeactivateObject(GameObject typePrefab, GameObject objectToAddToPool)
    {
        int poolKey = typePrefab.GetInstanceID();

        if (objectPoolDictionary.ContainsKey(poolKey))
        {
            objectPoolDictionary[poolKey].Enqueue(objectToAddToPool);
        }
    }

    public GameObject GetNextObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if(objectPoolDictionary.ContainsKey(poolKey))
        {
            GameObject newObject;

            if(objectPoolDictionary[poolKey].Count > 0)
            {
                //Return first object.
                newObject = objectPoolDictionary[poolKey].Dequeue();
            }
            else
            {
                //Instantiate another object of that type.
                newObject = Instantiate(prefab);
            }

            //Get object ready to be reused.
            newObject.transform.position = position;
            newObject.transform.rotation = rotation;
            newObject.SetActive(true);

            return newObject;
        }
        else //ERROR! NOT SUPPOSED TO REACH HERE.
        {
            return null;
        }
    }
}
