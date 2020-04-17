using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class spawns civilians
public class CivilianPopulation : MonoBehaviour
{
    //Store spawn locations for civilians
    public GameObject[] spawnLocations;
    //Stores our new civilian
    public GameObject tempCivilian;

    //Population size. This acts as the health for the base (Civilian)
    int popSize;
    int currentPop;

    void Start()
    {
        currentPop = 0;
        popSize = 5;
        SpawnCivilians();
    }

    public void SpawnCivilians()
    {
        while(currentPop < popSize)
        {
            //update population
            currentPop++;
            //find the prefab in resources
            tempCivilian = (GameObject)Resources.Load("Civilian", typeof(GameObject));
            //Create a random spawn location for the new civilian
            int spawn = Random.Range(0, spawnLocations.Length);
            //Instantiate
            GameObject.Instantiate(tempCivilian, spawnLocations[spawn].transform.position, Quaternion.identity);
        }
    }
}
