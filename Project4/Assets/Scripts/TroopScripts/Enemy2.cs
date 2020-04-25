using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Priorities: attack civilians -> defenders -> structures
public class Enemy2 : MonoBehaviour
{

    private Enemy2Population enemyManager;

    //list of houses
    public GameObject[] houses;
    public GameObject[] civilians; 
    int state = 1;
    int health = 100;
    int maxHealth = 100;

    [SerializeField] private HealthBarScript healthBar = null;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GameObject.FindGameObjectWithTag("Enemy2Manager").GetComponentInChildren<Enemy2Population>();
        houses = GameObject.FindGameObjectsWithTag("House");
        civilians = GameObject.FindGameObjectsWithTag("Civilian");
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 1)
            civilianEngageBattle();
    }

    //state 1. Enemies move towards the nearest structures
    void civilianEngageBattle()
    {

        checkHealth();
        bool searchCivilian = false;
        //There must me a civilian outside of a house.Loop through all of them to see
        civilians = GameObject.FindGameObjectsWithTag("Civilian");
        for (int i = 0; i < civilians.Length; i++)
        {
            if (!civilians[i].GetComponent<Civilian>().getInAHouse())
            {
                searchCivilian = true;
            }
        }

        if (searchCivilian)
        {
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


        //If there are no Civilians, attack the houses
        int closestHouse = -1;
        if (!searchCivilian)
        {

            //locate nearest house to bring the sitizen to
            float closestHouseDist = Mathf.Infinity;//Vector3.Distance(transform.position, houses[0].transform.position);
            float nextDistance;
            
            for (int i = 0; i < houses.Length; i++)
            {
                if (houses[i] != null) //TODO: See if this works as a patch. Really we need a better solution than this.
                {
                    nextDistance = Vector3.Distance(transform.position, houses[i].transform.position);
                    if (nextDistance < closestHouseDist)
                    {
                        closestHouseDist = nextDistance;
                        closestHouse = i;
                    }
                }
            }

            //move towards the closest civilian
            if (closestHouse != -1 && houses[closestHouse] != null)
            {
                transform.LookAt(houses[closestHouse].transform);
                GetComponent<Rigidbody>().AddForce(transform.forward * 3);
            }
        }
    }
    void checkHealth()
    {
        if (health <= 0)
        {
            enemyManager.DestroyAnEnemy(gameObject);
            Destroy(gameObject);
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
