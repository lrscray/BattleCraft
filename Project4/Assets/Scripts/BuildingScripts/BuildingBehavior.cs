using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehavior : MonoBehaviour
{
    [SerializeField] private string buildingManagerType = "";

    [SerializeField] private int buildingMaxHealth = -1;
    [SerializeField] private int buildingCurrentHealth = -1;
    [SerializeField] private int healAmount = -1; //The amount that the building heals, when it does heal.

    [SerializeField] private int buildingCreationCost = -1; //The amount of resources it takes to build this building.

    //[SerializeField] private float upKeepWaitPeriod = -1f; //The time to wait before attempting to take upkeep.
    //[SerializeField] private int buildingUpKeepCost = -1; //The number of resources it takes to upkeep this building.
    //[SerializeField] private int upKeepDamageAmount = -1; //The amount of damage the building takes if the upkeep cannot be met.

    //[SerializeField] private int maxNumTroopSpawn = -1; //The total number of troops that can be spawned from this building.
    //[SerializeField] private int numTroopsSpawned = -1; //The current number of troops spawned from this building.

    private bool spawningEnabled = false;
    [SerializeField] private bool shouldSpawnStartTroops = false;
    [SerializeField] private int numTroopsToSpawn = 0;
    [SerializeField] private float spawningWaitPeriod = -1f; //The time to wait before attempting to spawn troops.
    [SerializeField] private GameObject spawnPlacementObject = null;
    [SerializeField] private GameObject troopTypePrefab = null;
    [SerializeField] private int troopCreationCost = -1;

    [SerializeField] private int maxTroopCapacity = -1; //The total number of troops that can be fit inside this building.
    [SerializeField] private int currentNumInsideTroops = -1; //The current number of troops inside this building.
    private List<GameObject> insideTroops = null;

    private BuildingManager buildingManager = null;
    //TODO Consider changing how this is found.
    private TroopManager defenderManager = null;
    private TroopManager collectorManager = null;
    private TroopManager civilianManager = null;

    private void Start()
    {
        insideTroops = new List<GameObject>();

        //numTroopsSpawned = 0;
        buildingManager = GameObject.FindGameObjectWithTag(buildingManagerType).GetComponentInChildren<BuildingManager>();
        transform.SetParent(buildingManager.gameObject.transform);
        buildingManager.MakeBuilding(this.gameObject);
        civilianManager = GameObject.FindGameObjectWithTag("CivilianManager").GetComponentInChildren<TroopManager>();
        defenderManager = GameObject.FindGameObjectWithTag("DefenderManager").GetComponentInChildren<TroopManager>();
        collectorManager = GameObject.FindGameObjectWithTag("CollectorManager").GetComponentInChildren<TroopManager>();
        
        currentNumInsideTroops = 0;

        if(shouldSpawnStartTroops)
        {
            StartSpawningTroops();
        }
        //StartCoroutine(WaitSpawnTroops());
        //StartCoroutine(WaitTakeUpKeep());
    }

    public void StartSpawningTroops()
    {
        if(spawningEnabled == false)
        {
            spawningEnabled = true;
            StartCoroutine(WaitSpawnTroops());
        }
    }

    public void IncrementNumTroopsToSpawn()
    {
        numTroopsToSpawn++;
    }
    public void DecrementNumTroopsToSpawn()
    {
        if (numTroopsToSpawn > 0)
        {
            numTroopsToSpawn--;
        }
    }

    IEnumerator WaitSpawnTroops()
    {
        while (numTroopsToSpawn > 0)
        {
            yield return new WaitForSeconds(spawningWaitPeriod);
            if (numTroopsToSpawn > 0) //Make sure the player didnt decrement while waiting to spawn.
            {
                AttemptSpawnTroops();
            }
        }
        spawningEnabled = false;
    }

    /*
    IEnumerator WaitTakeUpKeep()
    {
        while (true)
        {
            yield return new WaitForSeconds(upKeepWaitPeriod);
            AttemptCollectUpKeep();
        }
    }
    */

    private void AttemptSpawnTroops()
    {
        //Check if there is enough resources to spawn troop.
        if(PlayerResourceManager.instance.GetNumResources() > troopCreationCost)
        {
            //If so, spawn troop.
            SpawnTroop();
        }
    }

    private void SpawnTroop()
    {
        PlayerResourceManager.instance.UseResources(troopCreationCost);
        //GameObject troop = Instantiate(troopTypePrefab, spawnPlacementObject.transform.position, spawnPlacementObject.transform.rotation);
        GameObject troop = ObjectPoolManager.instance.GetNextObject(troopTypePrefab, spawnPlacementObject.transform.position, spawnPlacementObject.transform.rotation);
        if(troop == null)
        {
            Debug.LogError("ERROR in HOUSINGBEHAVIOR!");
        }
        if(troopTypePrefab.tag == "Civilian")
        {
            troop.transform.SetParent(civilianManager.transform);
            civilianManager.AddTroop(troop);
        }
        else if(troopTypePrefab.tag == "Collector")
        {
            troop.transform.SetParent(collectorManager.transform);
            collectorManager.AddTroop(troop);
        }
        else if(troopTypePrefab.tag == "Defender")
        {
            troop.transform.SetParent(defenderManager.transform);
            defenderManager.AddTroop(troop);
        }
        DecrementNumTroopsToSpawn();
    }

    public int GetNumTroopsToSpawn()
    {
        return numTroopsToSpawn;
    }

    public int GetTroopCreationCost()
    {
        return troopCreationCost;
    }

    /*
    private void AttemptCollectUpKeep()
    {
        if(PlayerResourceManager.instance.GetNumResources() > buildingUpKeepCost)
        {
            PlayerResourceManager.instance.TakeUpKeep(buildingUpKeepCost);
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
    */

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

    public void StoreTroop(GameObject troop)
    {
        insideTroops.Add(troop);
        troop.SetActive(false);
        //ObjectPoolManager.instance.DeactivateObject(civilianManager.GetTroopPrefab(), troop);
        currentNumInsideTroops += 1;
    }

    //What happens when the building is destroyed.
    private void Die()
    {
        KickOutInsideTroops();
        buildingManager.DestroyBuilding(gameObject);
        //print("building broken");
        NavMeshManager.instance.UpdateNavMesh();
    }

    private void KickOutInsideTroops()
    {
        //Go through all inside troops.
        //Change position to center of this gameobject.
        //SetActive again.
        for(int i = 0; i < insideTroops.Count; i++)
        {
            //GameObject troop = ObjectPoolManager.instance.GetNextObject(civilianManager.GetTroopPrefab(), gameObject.transform.position, gameObject.transform.rotation);
            GameObject troop = insideTroops[i];
            troop.transform.position = gameObject.transform.position;
            troop.SetActive(true);
            Civilian civilian = troop.GetComponentInChildren<Civilian>();
            civilian.setInAHouse(false);
            civilian.setHasADefender(false);
            civilian.SetState(1);//Make them wander.
        }
    }

    public int GetBuildingCreationCost()
    {
        return buildingCreationCost;
    }

    /*
    public int GetBuildingUpKeepCost()
    {
        return buildingUpKeepCost;
    }
    */

    public bool HouseHasRoom()
    {
        if(GetCurrentNumInsideTroops() < maxTroopCapacity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetMaxTroopCapacity()
    {
        return maxTroopCapacity;
    }

    public int GetCurrentNumInsideTroops()
    {
        return currentNumInsideTroops;
    }

    public string GetBuildingManagerType()
    {
        return buildingManagerType;
    }

    //TODO: Consider changing how attacking works with enemies and buildings. Maybe use a distance system?
    void OnCollisionEnter(Collision collision)
    {
        //Attacked by defender
        if (collision.gameObject.tag == "EnemyBasic" || collision.gameObject.tag == "Enemy2")
        {
            TakeDamage(2);

            //print("House health: " + buildingCurrentHealth);
        }
    }
}
