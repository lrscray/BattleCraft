using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//NOTE: This class is deprecated! Use the ENEMY MANAGER script instead!
public class EnemyBasicPopulation : MonoBehaviour
{
    //Store spawn locations for civilians
    [SerializeField] private GameObject[] spawnLocations = null;
    //Stores our new civilian
    [SerializeField] private GameObject tempEnemyBasic = null;

    //Enemy Population size. 
    //private int maxPopSize; //DESIGN: Do we want to cap the number of enemies that can come at once?
    [SerializeField] private int numCurrentEnemies = 0;
    private List<GameObject> enemies = null;

    [SerializeField] private int numEnemiesInWave = 1; //The number of enemies in a wave. TODO: Change this to increase/change over time with difficulty.

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
            //Create a random spawn location for the new civilian
            int spawn = Random.Range(0, spawnLocations.Length);
            //Instantiate
            GameObject troop = Instantiate(tempEnemyBasic, spawnLocations[spawn].transform.position, Quaternion.identity);
            troop.transform.SetParent(transform);

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
