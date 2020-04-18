using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Defender: MonoBehaviour
{

    //TODO: Consider adding a detection collider for finding enemies.

    //list of points to patrol through
    public GameObject[] wanderPoints;
    //list of basic enemies
    public GameObject[] enemiesBasic;
    public GameObject[] enemies2;
    //list of houses
    public GameObject[] houses;
    //list of civilians
    public GameObject[] civilians;
    //list of defenders
    public GameObject[] defenders;
    private Enemy2Population enemyManager;

    int health = 100;
    int maxHealth = 100;

    [SerializeField]
    private HealthBarScript healthBar;

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
        //find all of the wander points when the object is created
        wanderPoints = GameObject.FindGameObjectsWithTag("DefenderWanderPoint");
        //enemiesBasic = GameObject.FindGameObjectsWithTag("EnemyBasic");
        houses = GameObject.FindGameObjectsWithTag("House");
        civilians = GameObject.FindGameObjectsWithTag("Civilian");
        defenders = GameObject.FindGameObjectsWithTag("Defender");
        enemies2 = GameObject.FindGameObjectsWithTag("Enemy2");
        enemyManager = GameObject.FindGameObjectWithTag("Enemy2Manager").GetComponentInChildren<Enemy2Population>();

        agent = GetComponent<NavMeshAgent>();
        //agent.autoBraking = false;

        int nextPoint = Random.Range(0, wanderPoints.Length);
        agent.destination = wanderPoints[nextPoint].transform.position;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        enemiesBasic = GameObject.FindGameObjectsWithTag("EnemyBasic");
        civilians = GameObject.FindGameObjectsWithTag("Civilian");

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
        //if we are close to our destination point, go to the next point
        if (!agent.pathPending && agent.remainingDistance < minRemainingDistance)
        {
            int nextPoint = Random.Range(0, wanderPoints.Length);
            agent.destination = wanderPoints[nextPoint].transform.position;
        }

        //If an enemy is spotted within a certain distance while in the wandering state, enter the escort state.
        for (int i = 0; i < enemiesBasic.Length; i++)
        {
            //calculate distance between the defender and the enemy
            distance = Vector3.Distance(transform.position, enemiesBasic[i].transform.position);

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
        //if all of the citizens are taken care of. go to state 4. Attack state
        bool noMoreCitizens = true;
        for (int j = 0; j < civilians.Length; j++)
        {
            if (civilians[j].gameObject.GetComponent<Renderer>().enabled == true) //&& civilians[j].GetComponent<Civilian>().getHasADefender() == false)
            {
                noMoreCitizens = false;
            }
        }
        if (noMoreCitizens)
        {
            state = 4;
        }
        civilians = GameObject.FindGameObjectsWithTag("Civilian");
        //find the nearest civilian that is not in a house
        float closestCivilianDist = 0;
        int closestCivilian = 0;
        float nextDistance;
        for (int j = 0; j < civilians.Length; j++)
        {
            if (civilians[j].gameObject.GetComponent<Renderer>().enabled == true) // && civilians[j].GetComponent<Civilian>().getHasADefender() == false
            {
                  if (!civilians[j].GetComponent<Civilian>().getHasADefender())
                  {
                        closestCivilianDist = Vector3.Distance(transform.position, civilians[j].transform.position);
                        closestCivilian = j;
                  }
            }
        }

        for (int i = 1; i < civilians.Length; i++)
        {
            nextDistance = Vector3.Distance(transform.position, civilians[i].transform.position);
            if (nextDistance < closestCivilianDist && civilians[i].gameObject.GetComponent<Renderer>().enabled == true)   //&& civilians[j].GetComponent<Civilian>().getHasADefender() == false
            {
                 if (!civilians[i].GetComponent<Civilian>().getHasADefender())
                 {
                        closestCivilianDist = nextDistance;
                        closestCivilian = i;
                  }
            }
        }
        //move towards the closest civilian
        transform.LookAt(civilians[closestCivilian].transform);
        GetComponent<Rigidbody>().AddForce(transform.forward * 9);
        //Once contact is made with the citizen. move them to the nearest house
    }
    //state 3
    void Escort()
    {
        //locate nearest house to bring the sitizen to
        float closestHouseDist = Vector3.Distance(transform.position, houses[0].transform.position);
        float nextDistance;
        int closestHouse = 0; ;
        for (int i = 1; i < houses.Length; i++)
        {
            nextDistance = Vector3.Distance(transform.position, houses[i].transform.position);
            if (nextDistance < closestHouseDist)
            {
                closestHouseDist = nextDistance;
                closestHouse = i;
            }
        }
        //move towards the closest civilian
        transform.LookAt(houses[closestHouse].transform);
        GetComponent<Rigidbody>().AddForce(transform.forward * 9);
        //Once contact is made with the citizen. move them to the nearest house
    }

    //state 4
    void Attack()
    {
        float tempDistance = 1000;
        float finalDistance = 1000;
        int closestEnemy = 0;

        enemiesBasic = null;
        enemiesBasic = GameObject.FindGameObjectsWithTag("EnemyBasic");
        int length = enemiesBasic.Length;
        if(enemiesBasic != null){
            //find the nearest enemy to attack
            for (int i = 1; i < enemiesBasic.Length; i++)
            {
                tempDistance = Vector3.Distance(transform.position, enemiesBasic[i].transform.position);
                if (tempDistance < finalDistance)
                {
                    finalDistance = tempDistance;
                    closestEnemy = i;
                }
            }
            //attack the enemy
            if (length != 0)
            {
                transform.LookAt(enemiesBasic[closestEnemy].transform);
                GetComponent<Rigidbody>().AddForce(transform.forward * 9);
            }
        }
    }

    void Attack2()
    {

        if (enemyManager.GetComponent<Enemy2Population>().GetCurrentPop() == 0)
        {
            state = 5;
        }

        float tempDistance = 1000;
        float finalDistance = 1000;
        int closestEnemy = 0;

        enemies2 = null;
        enemies2 = GameObject.FindGameObjectsWithTag("Enemy2");
        int length = enemies2.Length;
        if (enemies2 != null)
        {
            //find the nearest enemy to attack
            for (int i = 1; i < enemies2.Length; i++)
            {
                tempDistance = Vector3.Distance(transform.position, enemies2[i].transform.position);
                if (tempDistance < finalDistance)
                {
                    finalDistance = tempDistance;
                    closestEnemy = i;
                }
            }
            //attack the enemy
            if (length != 0)
            {
                transform.LookAt(enemies2[closestEnemy].transform);
                GetComponent<Rigidbody>().AddForce(transform.forward * 9);
            }
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
