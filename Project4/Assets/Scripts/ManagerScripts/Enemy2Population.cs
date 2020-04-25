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

    //Population size
    //private int popSize;
    //private int currentPop;
    private List<GameObject> enemies = null;

    [SerializeField] private int numEnemiesInWave = 1;

    void Start()
    {
        enemies = new List<GameObject>();

    }

    public void SpawnEnemy2()
    {
        for(int i = 0; i < numEnemiesInWave; i++)
        {
            //update population
            //currentPop++;
            //Create a random spawn location for the new civilian
            int spawn = Random.Range(0, spawnLocations.Length);
            //Instantiate
            GameObject troop = Instantiate(Enemy2, spawnLocations[spawn].transform.position, Quaternion.identity);
            troop.transform.SetParent(transform);
            AddAnEnemy(troop);
        }
    }

    public void AddAnEnemy(GameObject newEnemy)
    {
        enemies.Add(newEnemy);
    }
    public void DestroyAnEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
    /*
    public void DecrementCurrentPop()
    {
        currentPop--;
    }
    */

    public int GetNumCurrentTroops()
    {
        return enemies.Count;
    }

    public List<GameObject> GetAllEnemies()
    {
        return enemies;
    }
}
