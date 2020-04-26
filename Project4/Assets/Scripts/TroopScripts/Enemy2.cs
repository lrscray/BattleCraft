using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Priorities: attack civilians -> defenders -> structures
public class Enemy2 : MonoBehaviour
{
    private string troopManagerType = "Enemy2Manager";
    private TroopManager enemyManager;
    private TroopManager civilianManager;

    private BuildingManager civilianBuildingManager;
    private BuildingManager defenderBuildingManager;
    private BuildingManager collectorBuildingManager;

    //list of houses
    private List<GameObject> civilianBuildings = null;
    private List<GameObject> collectorBuildings = null;
    private List<GameObject> defenderBuildings = null;
    private List<GameObject> allBuildings = null;

    //list of civilians
    private List<GameObject> civilians = null;

    int state = 1;
    int health = 100;
    int maxHealth = 100;

    [SerializeField] private HealthBarScript healthBar = null;

    private Rigidbody enemyRigidBody;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GameObject.FindGameObjectWithTag(troopManagerType).GetComponentInChildren<TroopManager>();
        civilianManager = GameObject.FindGameObjectWithTag("CivilianManager").GetComponentInChildren<TroopManager>();

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
        enemyRigidBody = GetComponentInChildren<Rigidbody>();
        agent = gameObject.GetComponentInChildren<NavMeshAgent>();
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
        civilians = civilianManager.GetAllTroops();

        if (state == 1)
            civilianEngageBattle();
    }

    //state 1. Enemies move towards the nearest structures
    void civilianEngageBattle()
    {
        //Is this part necessary? Any way to have its function without looping through all civilians twice?
        
        bool searchCivilian = false;

        //There must me a civilian outside of a house.Loop through all of them to see
        int i = 0;
        while (searchCivilian == false && i < civilians.Count)
        {
            if (!civilians[i].GetComponent<Civilian>().getInAHouse())
            {
                searchCivilian = true;
            }
            i++;
        }
        

        if (searchCivilian)
        {
        //Looking for a civilian.

        float closestCivilianDist = Mathf.Infinity;
        int closestCivilianIndex = -1;
        float distance;
        for (int j = 0; j < civilians.Count; j++)
        {
            if (civilians[j].activeInHierarchy) // && civilians[j].GetComponent<Civilian>().getHasADefender() == false
            {
                //if (civilians[j].GetComponent<Civilian>().getHasADefender() == false)
                //{
                    distance = Vector3.Distance(transform.position, civilians[j].transform.position);
                    if (distance < closestCivilianDist)
                    {
                        closestCivilianDist = distance;
                        closestCivilianIndex = j;
                    }
                //}
            }
        }

        //move towards the closest civilian
        if (closestCivilianIndex != -1 && civilians[closestCivilianIndex] != null)
        {
            //transform.LookAt(civilians[closestCivilianIndex].transform);
            //GetComponent<Rigidbody>().AddForce(transform.forward * 9);
            agent.SetDestination(civilians[closestCivilianIndex].transform.position);
        }
        //Once contact is made with the citizen. move them to the nearest house
        }


        //If there are no Civilians, attack the houses
        GameObject nearestHouse = FindClosestThing(allBuildings);

        //move towards the closest civilian
        if (nearestHouse != null)
        {
            //transform.LookAt(nearestHouse.transform);
            //GetComponent<Rigidbody>().AddForce(transform.forward * 3);
            agent.SetDestination(nearestHouse.transform.position);
        }
        
    }
    void checkHealth()
    {
        if (health <= 0)
        {
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
            checkHealth();
        }
        if (collision.gameObject.tag == "House" || collision.gameObject.tag == "Home")
        {
            Vector3 dir = collision.contacts[0].point - transform.position;
            dir = -dir.normalized;

            enemyRigidBody.AddForce(dir * 500);
            
        }
    }


}
