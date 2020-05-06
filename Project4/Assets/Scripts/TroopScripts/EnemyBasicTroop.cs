using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBasicTroop : Troop
{ 

    private bool chasingOAttacking = false;
    //The distance between the target and this troop.
    private Vector3 distanceVector;


   new void Start()
    {
        GameObject InitialObjective = GameObject.FindGameObjectWithTag("Center");
        SetPreselectedDestination(InitialObjective.transform);
    }

   

    protected override void ReactToEnemy()
    {
        //Should run towards closest enemy and attack.
        GameObject closestEnemy = FindClosestThing(detectedObjects);
        if (closestEnemy != null)
        {
            //If not already chasing and attacking, do so.
            if (chasingOAttacking != true)
            {
                //Debug.Log(gameObject.name + " started chasing " + closestEnemy.name + "!");
                StartCoroutine(ChaseAndAttackEnemy(closestEnemy));
            }
        }
    }
    public override void InitializeTroop() {
        GameObject InitialObjective = GameObject.FindGameObjectWithTag("Center");
        SetPreselectedDestination(InitialObjective.transform);
    }


    private void ChaseEnemy(GameObject enemyObject)
    {
        distanceVector = transform.position - enemyObject.transform.position;
        Vector3 distanceVectorNormalized = distanceVector.normalized;
        Vector3 targetDistance = (distanceVectorNormalized * chaseOffset);
        navAgent.SetDestination(enemyObject.transform.position + targetDistance);
        //Debug.Log(gameObject.name + " is chasing " + enemyObject.name + "!");
    }

    IEnumerator ChaseAndAttackEnemy(GameObject target)
    {
        chasingOAttacking = true;
        while (target != null) //TODO: Think about using a different condition.
        {
            yield return 0;
            ChaseEnemy(target);

            //Debug.Log("Distance: " + distanceVector.magnitude);
            //If too far. Lose it.
            if (distanceVector.magnitude > detectionRadius)
            {
                target = null;
                break;
            }

            //If within attacking distance,
            if (distanceVector.magnitude <= attackRadius)
            {
                //Attack!
                //Target takes damage.
                //if() 

                Debug.Log(gameObject.name + " attacked " + target.name + "!");
                yield return new WaitForSeconds(attackTimeCooldown);
            }
        }
        chasingOAttacking = false;
    }




}