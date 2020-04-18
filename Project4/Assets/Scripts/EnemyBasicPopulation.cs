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

    //Enemy Population size. 
    private int maxPopSize; //DESIGN: Do we want to cap the number of enemies that can come at once?
    [SerializeField] private int numCurrentEnemies = 0;

    [SerializeField] private int numEnemiesInWave = 9; //The number of enemies in a wave. TODO: Change this to increase/change over time with difficulty.

    //private void Start()//TODO: Remove this and rely on WaveManager to call methods instead.
    //{
    //    SpawnWaveEnemies();        
    //}

    public void SpawnWaveEnemies() //TODO: Change this to work with a parameter for num of enemies to spawn?
    {
        for(int i = 0; i < numEnemiesInWave; i++)
        {
            //update population
            numCurrentEnemies++;
            //find the prefab in resources
            tempEnemyBasic = (GameObject)Resources.Load("EnemyBasic", typeof(GameObject));
            //Create a random spawn location for the new civilian
            int spawn = Random.Range(0, spawnLocations.Length);
            //Instantiate
            GameObject.Instantiate(tempEnemyBasic, spawnLocations[spawn].transform.position, Quaternion.identity);
        }
    }

    public int GetNumCurrentEnemies()
    {
        return numCurrentEnemies;
    }

    public void DecrementNumCurrentEnemies()
    {
        numCurrentEnemies--;
    }
}
