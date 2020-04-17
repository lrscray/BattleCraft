using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehavior : MonoBehaviour
{
    [SerializeField] private int buildingMaxHealth = -1;
    [SerializeField] private int buildingCurrentHealth = -1;
    [SerializeField] private int healAmount = -1; //The amount that the building heals, when it does heal.

    [SerializeField] private int buildingCreationCost = -1; //The amount of resources it takes to build this building.

    [SerializeField] private float upKeepWaitPeriod = -1f; //The time to wait before attempting to take upkeep.
    [SerializeField] private int buildingUpKeepCost = -1; //The number of resources it takes to upkeep this building.
    [SerializeField] private int upKeepDamageAmount = -1; //The amount of damage the building takes if the upkeep cannot be met.

    [SerializeField] private int maxTroopCapacity = -1; //The total number of troops that can be assigned to this building.
    [SerializeField] private int currentNumTroops = -1; //The current number of troops assigned to this building.

    private bool spawningEnabled = true;
    [SerializeField] private float spawningWaitPeriod = -1f; //The time to wait before attempting to spawn troops.
    [SerializeField] private GameObject troopTypePrefab = null;
    //TODO Prepare this futher for game.
    private List<GameObject> troopsArray;

    //TODO Consider changing how this is found.
    private PlayerResourceManager resourceManager = null;

    private void Start()
    {
        resourceManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<PlayerResourceManager>();
        troopsArray = new List<GameObject>();
        StartCoroutine(WaitSpawnTroops());
        StartCoroutine(WaitTakeUpKeep());
    }
    
    IEnumerator WaitSpawnTroops()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawningWaitPeriod);
            AttemptSpawnTroops();
        }
    }

    IEnumerator WaitTakeUpKeep()
    {
        while (true)
        {
            yield return new WaitForSeconds(upKeepWaitPeriod);
            AttemptCollectUpKeep();
        }
    }

    private void AttemptSpawnTroops()
    {
        if(currentNumTroops < maxTroopCapacity)
        {
            if(spawningEnabled)
            {
                //Spawn troop.
                SpawnTroop();
            }
        }
    }

    //TODO Change where the troops spawn.
    //TODO Add troops to troopsArray list of building to keep track of them.
    private void SpawnTroop()
    {
        Instantiate(troopTypePrefab, gameObject.transform.position, gameObject.transform.rotation);
        currentNumTroops++;
    }

    private void AttemptCollectUpKeep()
    {
        if(resourceManager.GetNumResources() > buildingUpKeepCost)
        {
            resourceManager.TakeUpKeep(buildingUpKeepCost);
            spawningEnabled = true;
            if(buildingCurrentHealth < buildingMaxHealth)
            {
                Heal();
            }
        }
        else
        {
            TakeUpKeepDamage();
            spawningEnabled = false;
        }
    }

    private void TakeUpKeepDamage()
    {
        buildingCurrentHealth -= upKeepDamageAmount;
        if(buildingCurrentHealth < 0)
        {
            //Destroy building.
            Die();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        buildingCurrentHealth -= damageAmount;
        if (buildingCurrentHealth < 0)
        {
            //Destroy building.
            Die();
        }
    }

    private void Heal()
    {
        if(buildingMaxHealth - buildingCurrentHealth <= healAmount)
        {
            buildingCurrentHealth = buildingMaxHealth;
        }
        else
        {
            buildingCurrentHealth += healAmount;
        }
    }

    //TODO Maybe change this to force out all troops!
    //What happens when the building is destroyed.
    private void Die()
    {
        Destroy(gameObject);
    }

    public int GetBuildingCreationCost()
    {
        return buildingCreationCost;
    }

    public int GetBuildingUpKeepCost()
    {
        return buildingUpKeepCost;
    }

    public int GetMaxTroopCapacity()
    {
        return maxTroopCapacity;
    }

    public int GetCurrentNumTroops()
    {
        return currentNumTroops;
    }
}
