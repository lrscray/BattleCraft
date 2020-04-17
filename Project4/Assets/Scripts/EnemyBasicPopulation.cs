using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class spawns civilians
public class EnemyBasicPopulation : MonoBehaviour
{
    //Store spawn locations for civilians
    public GameObject[] spawnLocations;
    //Stores our new civilian
    public GameObject tempEnemyBasic;

    //Population size. This acts as the health for the base (Civilian)
    int popSize;
    int currentPop;

    void Start()
    {
        currentPop = 0;
        popSize = 9;
        SpawnEnemyBasic();
    }

    public void SpawnEnemyBasic()
    {
        while (currentPop < popSize)
        {
            //update population
            currentPop++;
            //find the prefab in resources
            tempEnemyBasic = (GameObject)Resources.Load("EnemyBasic", typeof(GameObject));
            //Create a random spawn location for the new civilian
            int spawn = Random.Range(0, spawnLocations.Length);
            //Instantiate
            GameObject.Instantiate(tempEnemyBasic, spawnLocations[spawn].transform.position, Quaternion.identity);
        }
    }
}
