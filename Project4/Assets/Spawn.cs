using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject Collector;
    [SerializeField] private float spawnDelay = 10.0f;
    //[SerializeField] private bool stopSpawn = false;

    private int household;
    private float WaitTime;

    void Update()
    {
        if (household < 5)
        {
            SpawnIn();
        }
    }
        
    void SpawnCount()
    {
        household++;
    }

    void SpawnIn()
    {
        WaitTime = Time.time + spawnDelay;
        Instantiate(Collector, transform.position, Quaternion.identity);
        SpawnCount();
    }



}
