using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBasic : MonoBehaviour
{
    private string troopManagerType = "EnemyManager";
    private TroopManager enemyManager;

    private BuildingManager civilianBuildingManager;
    private BuildingManager defenderBuildingManager;
    private BuildingManager collectorBuildingManager;

    //list of houses
    private List<GameObject> civilianBuildings = null;
    private List<GameObject> collectorBuildings = null;
    private List<GameObject> defenderBuildings = null;
    private List<GameObject> allBuildings = null;

    int state = 1;
    int health = 100;
    int maxHealth = 100;

    
    [SerializeField] private HealthBarScript healthBar = null;


    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GameObject.FindGameObjectWithTag(troopManagerType).GetComponentInChildren<TroopManager>();
        civilianBuildingManager = GameObject.FindGameObjectWithTag("CivilianBuildingManager").GetComponentInChildren<BuildingManager>();
        defenderBuildingManager = GameObject.FindGameObjectWithTag("DefenderBuildingManager").GetComponentInChildren<BuildingManager>();
        collectorBuildingManager = GameObject.FindGameObjectWithTag("CollectorBuildingManager").GetComponentInChildren<BuildingManager>();

        civilianBuildings = civilianBuildingManager.GetAllBuildings();
        defenderBuildings = defenderBuildingManager.GetAllBuildings();
        collectorBuildings = collectorBuildingManager.GetAllBuildings();
        allBuildings = new List<GameObject>();
        allBuildings.AddRange(civilianBuildings);
        allBuildings.AddRange(defenderBuildings);
        allBuildings.AddRange(collectorBuildings);
        
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        civilianBuildings = civilianBuildingManager.GetAllBuildings();
        defenderBuildings = defenderBuildingManager.GetAllBuildings();
        collectorBuildings = collectorBuildingManager.GetAllBuildings();
        allBuildings.Clear();
        allBuildings.AddRange(civilianBuildings);
        allBuildings.AddRange(defenderBuildings);
        allBuildings.AddRange(collectorBuildings);


        if (state == 1)
            enemyEngageBattle();

    }

    //state 1. Enemies move towards the nearest structures
    void enemyEngageBattle()
    {
        checkHealth();

        //locate nearest house to bring the sitizen to
        GameObject nearestBuilding = FindClosestThing(allBuildings);

        //move towards the closest civilian
        if (nearestBuilding != null)
        {
            transform.LookAt(nearestBuilding.transform);
            GetComponent<Rigidbody>().AddForce(transform.forward * 3);
        }
        //Once contact is made with the citizen. move them to the nearest house
    }
    void checkHealth()
    {
        if (health <= 0)
        {
            //Update enemy manager to subtract total num enemies.
            enemyManager.DestroyTroop(gameObject);
            Destroy(gameObject);
        }
    }

    private GameObject FindClosestThing(List<GameObject> thingList)
    {
        float closestDistance = Mathf.Infinity;
        float distance;
        int closestThingIndex = -1;
        for (int i = 0; i < thingList.Count; i++)
        {
            if (thingList[i] != null)
            {
                distance = Vector3.Distance(transform.position, thingList[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestThingIndex = i;
                }
            }
        }

        if (closestThingIndex != -1 && thingList[closestThingIndex] != null)
        {
            return thingList[closestThingIndex];
        }
        else
        {
            return null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Attacked by defender
        if (collision.gameObject.tag == "Defender")
        {
            health = health - 10;
            healthBar.SetHealth(health);
        }
    }


}









