using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Defender: MonoBehaviour
{
    //TODO: Consider adding a detection collider for finding enemies.

    private BuildingManager civilianBuildingManager;
    private TroopManager civilianManager;
    private TroopManager enemyManager;
    private TroopManager enemy2Manager;

    //list of points to patrol through
    public GameObject[] wanderPoints;
    
    //list of enemies
    public List<GameObject> enemies;
    private List<GameObject> enemies2;

    //list of houses
    private List<GameObject> civilianHouses = null;

    //list of civilians
    private List<GameObject> civilians = null;
    
    //list of defenders
    //public GameObject[] defenders;

    int health = 100;
    int maxHealth = 100;

    [SerializeField] private HealthBarScript healthBar = null;

    public int currentCivilian = 0;

    //Distance in which the Civilian locates their new point to wonder to
    //Solves issue with civilians bumping into each other at a point
    [SerializeField] private float minRemainingDistance = 0.5f;

    private int destinationPoint = 0;
    private NavMeshAgent agent;
    private float distance;

    int state = 1;

    void Start()
    {
        GetComponent<NavMeshAgent>().speed = 7;
        civilianBuildingManager = GameObject.FindGameObjectWithTag("CivilianBuildingManager").GetComponentInChildren<BuildingManager>();
        civilianManager = GameObject.FindGameObjectWithTag("CivilianManager").GetComponentInChildren<TroopManager>();
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponentInChildren<TroopManager>();
        enemy2Manager = GameObject.FindGameObjectWithTag("Enemy2Manager").GetComponentInChildren<TroopManager>();

        //find all of the wander points when the object is created
        wanderPoints = GameObject.FindGameObjectsWithTag("DefenderWanderPoint");
        
        civilianHouses = civilianBuildingManager.GetAllBuildings();
        civilians = civilianManager.GetAllTroops();
        enemies = enemyManager.GetAllTroops();
        enemies2 = enemy2Manager.GetAllTroops();

        agent = GetComponent<NavMeshAgent>();
        //agent.autoBraking = false;

        int nextPoint = Random.Range(0, wanderPoints.Length);
        //agent.destination = wanderPoints[nextPoint].transform.position;
        agent.SetDestination(wanderPoints[nextPoint].transform.position);
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        civilianHouses = civilianBuildingManager.GetAllBuildings();
        civilians = civilianManager.GetAllTroops();
        enemies = enemyManager.GetAllTroops();
        enemies2 = enemy2Manager.GetAllTroops();

        if (state == 1)
            Wander();
        if (state == 2)
            searchForCitizens();
        if (state == 3)
            Escort();
        //attacks regular enemies
        if (state == 5)
            Attack();
        //attacks enemy2
        if (state == 4)
            Attack2();

         checkHealth();
    }
    //state 1
    void Wander()
    {
        GetComponent<NavMeshAgent>().speed = 7;
        //if we are close to our destination point, go to the next point
        if (!agent.pathPending && agent.remainingDistance < minRemainingDistance)
        {
            int nextPoint = Random.Range(0, wanderPoints.Length);
            //agent.destination = wanderPoints[nextPoint].transform.position;
            agent.SetDestination(wanderPoints[nextPoint].transform.position);
        }

        //If an enemy is spotted within a certain distance while in the wandering state, enter the escort state.
        for (int i = 0; i < enemies.Count; i++)
        {
            //calculate distance between the defender and the enemy
            distance = Vector3.Distance(transform.position, enemies[i].transform.position);

            if (distance < 65)
            {
                state = 2;
                //transform.LookAt(EnemiesBasic[i].transform);
                //GetComponent<Rigidbody>().AddForce(transform.forward * 10);
            }
        }
    }

    //state 2
    void searchForCitizens()
    {
        GetComponent<NavMeshAgent>().speed = 12;
        //if all of the citizens are taken care of. go to state 4. Attack state
        //also go to state 4 if there are citizens but no houses to put them in
        bool noMoreCitizens = true;
        for (int j = 0; j < civilians.Count; j++)
        {
            if (civilians[j].activeInHierarchy == true)
            {
                noMoreCitizens = false;
            }
        }

        bool noMoreHouses = true;
        for (int j = 0; j < civilianHouses.Count; j++)
        {
            if (civilianHouses[j].activeInHierarchy == true)
            {
                noMoreHouses = false;
            }
        }

        if (noMoreCitizens || noMoreHouses)
        {
            state = 4;
        }
        else //Go get the citizens!
        {
            //civilians = GameObject.FindGameObjectsWithTag("Civilian");
            //find the nearest civilian that is not in a house
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
                //transform.LookAt(civilians[closestCivilian].transform);
                //GetComponent<Rigidbody>().AddForce(transform.forward * 9);
                agent.SetDestination(civilians[closestCivilian].transform.position);
            }
            //Once contact is made with the citizen. move them to the nearest house
        }
    }
    //state 3
    void Escort()
    {
        GetComponent<NavMeshAgent>().speed = 12;
        //locate nearest house to bring the sitizen to
        GameObject nearestHouse = FindClosestThing(civilianHouses);

        //move towards the closest civilian
        if (nearestHouse != null)
        {
            //transform.LookAt(nearestHouse.transform);
            //GetComponent<Rigidbody>().AddForce(transform.forward * 3);
            agent.SetDestination(nearestHouse.transform.position);
        }
        //Once contact is made with the citizen. move them to the nearest house
    }

    //state 4
    void Attack()
    {
        GetComponent<NavMeshAgent>().speed = 10;
        //find the nearest enemy to attack
        GameObject nearestEnemy = FindClosestThing(enemies);

        //attack the enemy
        if (nearestEnemy != null)
        {
            //transform.LookAt(nearestEnemy.transform);
            //GetComponent<Rigidbody>().AddForce(transform.forward * 9);
            agent.SetDestination(nearestEnemy.transform.position);
        }
        
    }

    void Attack2()
    {
        GetComponent<NavMeshAgent>().speed = 10;

        if (enemy2Manager.GetNumTroops() == 0)
        {
            state = 5;
        }

        //find the nearest enemy to attack
        GameObject nearestEnemy = FindClosestThing(enemies2);
        //attack the enemy
        if (nearestEnemy != null)
        {
            //transform.LookAt(nearestEnemy.transform);
            //GetComponent<Rigidbody>().AddForce(transform.forward * 9);
            agent.SetDestination(nearestEnemy.transform.position);
        }
        
    }


    //will not return anything if the closest thing is too far away.
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

        if (closestDistance > 50)
        {
            state = 5;
            return null;
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

    void checkHealth()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //collision into civilian while in state 2. Change to state 3(escorting state)
        if (collision.gameObject.tag == "Civilian" && state == 2)
        {
            state = 3;
        }
        //collision into houuse while in state 3. move to state 2
        if (collision.gameObject.tag == "House" && state == 3)
        {
            state = 2;
        }
        //Attacked by Enemy
        if (collision.gameObject.tag == "Enemy2")
        {
            health = health - 5;
            healthBar.SetHealth(health);
        }
        if (collision.gameObject.tag == "EnemyBasic")
        {
            health = health - 5;
            healthBar.SetHealth(health);
        }
    }
    public int getState()
    {
        return state;
    }
    public int getCurrentCivilian()
    {
        return currentCivilian;
    }
}
