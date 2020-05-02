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

    [SerializeField] private float upKeepWaitPeriod = -1f; //The time to wait before attempting to take upkeep.
    [SerializeField] private int buildingUpKeepCost = -1; //The number of resources it takes to upkeep this building.
    [SerializeField] private int upKeepDamageAmount = -1; //The amount of damage the building takes if the upkeep cannot be met.

    //TODO: If we ever get to the point of selecting buildings and making troops that way, remove these spawning variables.
    [SerializeField] private int maxNumTroopSpawn = -1; //The total number of troops that can be spawned from this building.
    [SerializeField] private int numTroopsSpawned = -1; //The current number of troops spawned from this building.

    private bool spawningEnabled = true;
    [SerializeField] private float spawningWaitPeriod = -1f; //The time to wait before attempting to spawn troops.
    [SerializeField] private GameObject spawnPlacementObject = null;
    [SerializeField] private GameObject troopTypePrefab = null;

    //The troops currently inside this building.
    private List<GameObject> insideTroops;
    [SerializeField] private int maxTroopCapacity = -1; //The total number of troops that can be fit inside this building.
    //[SerializeField] private int currentNumTroops = -1; //The current number of troops assigned to this building.


    private BuildingManager buildingManager = null;
    //TODO Consider changing how this is found.
    private TroopManager defenderManager = null;
    //MAYBE: Make a collector manager script?
    private GameObject collectorManager = null;
    private TroopManager civilianManager = null;
    private PlayerResourceManager resourceManager = null;   
    private NavMeshManager navMeshManager = null;


    private void Start()
    {
        numTroopsSpawned = 0;
        buildingManager = GameObject.FindGameObjectWithTag(buildingManagerType).GetComponentInChildren<BuildingManager>();
        transform.SetParent(buildingManager.gameObject.transform);
        buildingManager.MakeBuilding(this.gameObject);
        resourceManager = GameObject.FindGameObjectWithTag("ResourceManager").GetComponentInChildren<PlayerResourceManager>();
        civilianManager = GameObject.FindGameObjectWithTag("CivilianManager").GetComponentInChildren<TroopManager>();
        defenderManager = GameObject.FindGameObjectWithTag("DefenderManager").GetComponentInChildren<TroopManager>();
        collectorManager = GameObject.FindGameObjectWithTag("CollectorManager");
        navMeshManager = GameObject.FindGameObjectWithTag("NavMesh").GetComponentInChildren<NavMeshManager>();

        insideTroops = new List<GameObject>();

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
        if(numTroopsSpawned < maxNumTroopSpawn)
        {
            if(spawningEnabled)
            {
                //Spawn troop.
                SpawnTroop();
            }
        }
    }

    private void SpawnTroop()
    {
        GameObject troop = Instantiate(troopTypePrefab, spawnPlacementObject.transform.position, spawnPlacementObject.transform.rotation);
        if(troopTypePrefab.tag == "Civilian")
        {
            troop.transform.SetParent(civilianManager.transform);
            civilianManager.AddTroop(troop);
        }
        else if(troopTypePrefab.tag == "Collector")
        {
            troop.transform.SetParent(collectorManager.transform);
        }
        else if(troopTypePrefab.tag == "Defender")
        {
            troop.transform.SetParent(defenderManager.transform);
        }
        numTroopsSpawned++;
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

    public void StoreTroop(GameObject troop)
    {
        insideTroops.Add(troop);
    }

    //What happens when the building is destroyed.
    private void Die()
    {
        KickOutInsideTroops();
        buildingManager.DestroyBuilding(gameObject);
        Destroy(gameObject);
        //print("building broken");
        navMeshManager.UpdateNavMesh();
    }

    private void KickOutInsideTroops()
    {
        //Go through all inside troops.
        //Change position to center of this gameobject?
        //SetActive again.
        for(int i = 0; i < insideTroops.Count; i++)
        {
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

    public int GetBuildingUpKeepCost()
    {
        return buildingUpKeepCost;
    }

    public bool HouseHasRoom()
    {
        if(GetCurrentNumTroops() < maxTroopCapacity)
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

    public int GetCurrentNumTroops()
    {
        return insideTroops.Count;
    }

    public string GetBuildingManagerType()
    {
        return buildingManagerType;
    }

    //TODO: Consider changing how attacking works with enemies and buildings. Maybe use a distance system?
    void OnCollisionEnter(Collision collision)
    {
        //Attacked by defender
        if (collision.gameObject.tag == "EnemyBasic")
        {
            TakeDamage(10);

            //print("House health: " + buildingCurrentHealth);
        }
    }
}
