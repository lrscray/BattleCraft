using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class spawns civilians
public class Enemy2Population : MonoBehaviour
{
    //Store spawn locations for civilians
    public GameObject[] spawnLocations;
    //Stores our new civilian
    public GameObject Enemy2;

    //Population size. This acts as the health for the base (Civilian)
    int popSize;
    int currentPop;

    void Start()
    {
        currentPop = 0;
        popSize = 3;
        SpawnEnemy2();
    }

    public void SpawnEnemy2()
    {
        while (currentPop < popSize)
        {
            //update population
            currentPop++;
            //find the prefab in resources
            Enemy2 = (GameObject)Resources.Load("Enemy2", typeof(GameObject));
            //Create a random spawn location for the new civilian
            int spawn = Random.Range(0, spawnLocations.Length);
            //Instantiate
            GameObject.Instantiate(Enemy2, spawnLocations[spawn].transform.position, Quaternion.identity);
        }
    }

    public void DecrementCurrentPop()
    {
        currentPop--;
    }

    public int getCurrentPop()
    {
        return currentPop;
    }
}
