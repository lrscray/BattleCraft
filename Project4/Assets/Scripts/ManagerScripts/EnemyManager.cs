using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private string enemyManagerType = null; //What type of enemies are stored here?

    //Store spawn locations for spawning
    [SerializeField] private GameObject[] spawnLocations = null;

    [SerializeField] private GameObject enemyPrefab = null;

    private List<GameObject> enemies;

    [SerializeField] private int numEnemiesInWave = 1; //The number of enemies to spawn in 1 wave.

    // Start is called before the first frame update
    private void Start()
    {
        enemies = new List<GameObject>();
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i < numEnemiesInWave; i++)
        {
            //Create a random spawn location for the new civilian
            int spawn = Random.Range(0, spawnLocations.Length);
            //Instantiate
            GameObject troop = Instantiate(enemyPrefab, spawnLocations[spawn].transform.position, Quaternion.identity);
            troop.transform.SetParent(transform);
            AddEnemy(troop);
        }
    }

    public List<GameObject> GetAllEnemies()
    {
        return enemies;
    }

    public int GetNumEnemies()
    {
        return enemies.Count;
    }

    public void AddEnemy(GameObject newEnemy)
    {
        enemies.Add(newEnemy);
    }

    public void DestroyEnemy(GameObject destroyedEnemy)
    {
        enemies.Remove(destroyedEnemy);
    }

    public string GetEnemyManagerType()
    {
        return enemyManagerType;
    }
}
