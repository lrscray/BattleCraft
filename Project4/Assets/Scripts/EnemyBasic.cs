using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBasic : MonoBehaviour
{
    private EnemyBasicPopulation enemyManager;

    //list of houses
    public GameObject[] houses;
    int state = 1;
    int health = 100;
    int maxHealth = 100;

    
    [SerializeField]
    private HealthBarScript healthBar;


    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponentInChildren<EnemyBasicPopulation>();
        houses = GameObject.FindGameObjectsWithTag("House");
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 1)
            enemyEngageBattle();

    }

    //state 1. Enemies move towards the nearest structures
    void enemyEngageBattle()
    {
        checkHealth();

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
        GetComponent<Rigidbody>().AddForce(transform.forward * 3);
        //Once contact is made with the citizen. move them to the nearest house
    }
    void checkHealth()
    {
        if (health <= 0)
        {
            //Update enemy manager to subtract total num enemies.
            enemyManager.DecrementNumCurrentEnemies();
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









