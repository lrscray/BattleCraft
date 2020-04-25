using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class spawns civilians
public class Enemy2Population : MonoBehaviour
{
    //Store spawn locations for civilians
    [SerializeField] private GameObject[] spawnLocations = null;
    //Stores our new civilian
    [SerializeField] private GameObject Enemy2 = null;

    //Population size. This acts as the health for the base (Civilian)
    private int popSize;
    private int currentPop;

    [SerializeField] private int numEnemiesInWave = 1;

    void Start()
    {
        //currentPop = 0;
        //popSize = 3;
        //SpawnEnemy2();
    }

    public void SpawnEnemy2()
    {
        for(int i = 0; i < numEnemiesInWave; i++)
        {
            //update population
            currentPop++;
            //Create a random spawn location for the new civilian
            int spawn = Random.Range(0, spawnLocations.Length);
            //Instantiate
            GameObject troop = Instantiate(Enemy2, spawnLocations[spawn].transform.position, Quaternion.identity);
            troop.transform.SetParent(transform);
        }
    }

    public void DecrementCurrentPop()
    {
        currentPop--;
    }

    public int GetCurrentPop()
    {
        return currentPop;
    }
}
