using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager = null;

    [SerializeField] private string troopManagerType = null; //What type of troops are stored here?

    //Store spawn locations for spawning
    [SerializeField] private GameObject[] spawnLocations = null;

    [SerializeField] private GameObject troopPrefab = null;

    private List<GameObject> troops;

    [SerializeField] private int numTroopsInWave = 0; //The number of troops to spawn in 1 wave or the number of starting troops to spawn.

    // Start is called before the first frame update
    private void Start()
    {
        ObjectPoolManager.instance.CreateNewObjectPool(troopPrefab, 15);
        troops = new List<GameObject>();
    }

    public void SpawnTroops()
    {
        for (int i = 0; i < numTroopsInWave; i++)
        {
            //Create a random spawn location for the new civilian
            int spawn = Random.Range(0, spawnLocations.Length);
            //Instantiate
            GameObject troop = Instantiate(troopPrefab, spawnLocations[spawn].transform.position, Quaternion.identity);
            troop.transform.SetParent(transform);
            AddTroop(troop);
        }
    }

    public List<GameObject> GetAllTroops()
    {
        return troops;
    }

    public int GetNumTroops()
    {
        return troops.Count;
    }

    public void AddTroop(GameObject newEnemy)
    {
        troops.Add(newEnemy);
        if(troopManagerType == "CivilianManager")
        {
            levelManager.SetPlayerBaseMaxHealth(numTroopsInWave + 1);
        }
    }

    public void DestroyTroop(GameObject destroyedEnemy)
    {
        ObjectPoolManager.instance.DeactivateObject(troopPrefab, destroyedEnemy);
        troops.Remove(destroyedEnemy);
        if (troopManagerType == "CivilianManager")
        {
            levelManager.UpdatePlayerBaseHealth();
        }
    }

    public string GetTroopManagerType()
    {
        return troopManagerType;
    }
}
