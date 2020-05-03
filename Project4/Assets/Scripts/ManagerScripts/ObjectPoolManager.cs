using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    //Used to store and find the specific object pool for a specific object.
    Dictionary<int, Queue<GameObject>> ObjectPoolDictionary = new Dictionary<int, Queue<GameObject>>();

    public void CreateNewObjectPool(GameObject prefab, int poolSize)
    {
        
    }
}
