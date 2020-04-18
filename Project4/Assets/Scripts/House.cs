using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    int health = 100;

    void Update()
    {
        checkHealth();
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
        //Attacked by defender
        if (collision.gameObject.tag == "EnemyBasic")
        {
            health = health - 10;
            print("House health: " + health);
        }
    }
}
