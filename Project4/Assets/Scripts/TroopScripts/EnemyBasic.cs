using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBasic : MonoBehaviour
{
    private string troopManagerType = "EnemyManager";
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

        civilianManager = GameObject.FindGameObjectWithTag("CivilianManager").GetComponentInChildren<TroopManager>();

        enemyManager = GameObject.FindGameObjectWithTag(troopManagerType).GetComponentInChildren<TroopManager>();
        civilianBuildingManager = GameObject.FindGameObjectWithTag("CivilianBuildingManager").GetComponentInChildren<BuildingManager>();
        defenderBuildingManager = GameObject.FindGameObjectWithTag("DefenderBuildingManager").GetComponentInChildren<BuildingManager>();
        collectorBuildingManager = GameObject.FindGameObjectWithTag("CollectorBuildingManager").GetComponentInChildren<BuildingManager>();

        civilianBuildings = civilianBuildingManager.GetAllBuildings();
        defenderBuildings = defenderBuildingManager.GetAllBuildings();
        collectorBuildings = collectorBuildingManager.GetAllBuildings();
        civilians = civilianManager.GetAllTroops();
        allBuildings = new List<GameObject>();
        allBuildings.AddRange(civilianBuildings);
        allBuildings.AddRange(defenderBuildings);
        allBuildings.AddRange(collectorBuildings);


        healthBar.SetMaxHealth(maxHealth);
        enemyRigidBody = gameObject.GetComponentInChildren<Rigidbody>();
        agent = gameObject.GetComponentInChildren<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        civilians = civilianManager.GetAllTroops();
        //TODO: Find way to make this check less for performance? Only when a building is destroyed? 
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

        //locate nearest house to bring the sitizen to
        GameObject nearestBuilding = FindClosestThing(allBuildings);

        //move towards the closest civilian
        //TODO: Find way of making this calc less? //IDK if this would work. We want to recalc the path often so that if we place things in front of them, they dont get stuck..
        if (nearestBuilding != null) //&& CheckDestinationSame(nearestBuilding.transform.position) == false) 
        {
            //transform.LookAt(nearestBuilding.transform);
            //enemyRigidBody.AddForce(transform.forward * 3);
            agent.SetDestination(nearestBuilding.transform.position);
        }
        else
        {
            FindCivilian();
        }
        //Once contact is made with the citizen. move them to the nearest house
    }
    void checkHealth()
    {
        if (health <= 0)
        {
            //Update enemy manager.
            enemyManager.DestroyTroop(gameObject);
            Destroy(gameObject);
        }
    }

    private bool CheckDestinationSame(Vector3 newPos)
    {
        if (agent.destination == newPos)
        {
            return true;
        }
        else
        {
            return false;
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

    void FindCivilian()
    {
        GetComponent<NavMeshAgent>().speed = 6;
            float closestCivilianDist = Mathf.Infinity;
            int closestCivilian = -1;
            float distance;
            for (int j = 0; j < civilians.Count; j++)
            {
                if (civilians[j].activeInHierarchy == true) // && civilians[j].GetComponent<Civilian>().getHasADefender() == false
                {
                    if (civilians[j].GetComponent<Civilian>().getHasADefender() == false)
                    {
                        distance = Vector3.Distance(transform.position, civilians[j].transform.position);
                        if (distance < closestCivilianDist)
                        {
                            closestCivilianDist = distance;
                            closestCivilian = j;
                        }
                    }
                }
            }

            //move towards the closest civilian
            if (closestCivilian != -1 && civilians[closestCivilian] != null)
            {
                agent.SetDestination(civilians[closestCivilian].transform.position);
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