using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Troop : MonoBehaviour
{
    //Troop states:
    /* 1. Selected (by the player).
     * 2. Wander (around in a circle or through a list of points?
     * 3. Chasing
     * 4. Attacking
     */

    [SerializeField] protected NavMeshAgent navAgent = null;

    [SerializeField] private bool shouldWander = true; //Whether this troop should wander or just stand still.
    [SerializeField] private int wanderCircleRadius = 5; //The radius at which the troop wanders around.
    [SerializeField] private Transform wanderCircleCenter = null; //The center of the circle from where the troop wanders.

    private bool hasDetectedEnemy = false; //If this troop detects what it considers to be an enemy.
    [SerializeField] private List<string> enemyTags = null;
    [SerializeField] protected int detectionRadius = 20; //Make sure this value is same as detection collider radius.
    protected List<GameObject> detectedObjects = null;

    [SerializeField] protected int chaseOffset = 2; //The distance this troop will stand from things it chases.

    [SerializeField] protected int attackRadius = 2; //The radius that enemies must be in to be attacked by this troop.
    [SerializeField] protected float attackTimeCooldown = -1; //The cooldown time between attacks by this enemy.

    private bool isSelected = false; //Whether or not the player has selected this troop.
    private bool selectedDestinationSet = false;

    bool movingTowardsPresetDestination = false;

    // Start is called before the first frame update

    protected void Awake()
    {
        detectedObjects = new List<GameObject>();
    }

    protected void Start()
    {
        //patrolList = new List<Transform>(); //Needed?
        //detectedObjects = new List<GameObject>();
    }

    private void Update()
    {
        if (!movingTowardsPresetDestination)
        {
            //If the troop hasnt had the player select it and set a destination, 
            if (!selectedDestinationSet)
            {
                //Check if enemy nearby.
                CheckForEnemies();
                if (!hasDetectedEnemy)
                {
                    //If not, wander.
                    Wander();
                }
                else
                {
                    //Enemy is nearby, so react to them!
                    ReactToEnemy();
                }
            }
            else
            {
                //Move to selected destination.
                MoveToSelectedDestination();
            }
        }
        else
        {
            MoveToPreselectedDestination();
        }
    }

    protected void MoveToPreselectedDestination()
    {
        if (navAgent.remainingDistance < 0.5f)
        {
            //If at destination, go back to normal behavior.
            wanderCircleCenter.position = gameObject.transform.position;
            movingTowardsPresetDestination = false;

        }
    }

    protected void SetPreselectedDestination(Transform newDestination)
    {
        movingTowardsPresetDestination = true;
        navAgent.SetDestination(newDestination.position);
    }

    protected void MoveToSelectedDestination()
    {
        //Check if reached destination.
        if (navAgent.remainingDistance < 0.5f)
        {
            //If at destination, go back to normal behavior.
            wanderCircleCenter.position = gameObject.transform.position;
            selectedDestinationSet = false;

        }
    }

    protected void SetSelectedDestination(Transform newDestination)
    {
        selectedDestinationSet = true;
        navAgent.SetDestination(newDestination.position);
    }

    protected void Wander()
    {
        if (shouldWander)
        {
            if (navAgent.remainingDistance < 0.5f)
            {
                Vector2 nextWanderPoint = Random.insideUnitCircle.normalized;
                //Navigate to the next wander point!                               //TODO: Check vector math!
                navAgent.SetDestination(new Vector3(wanderCircleCenter.position.x + (wanderCircleRadius * nextWanderPoint.x), wanderCircleCenter.position.y, wanderCircleCenter.position.z + (wanderCircleRadius * nextWanderPoint.y)));

            }
        }
    }

    protected void CheckForEnemies()
    {
        int numEnemiesDetected = 0;
        for (int i = 0; i < detectedObjects.Count; i++)
        {
            if (enemyTags.Contains(detectedObjects[i].tag))
            {
                numEnemiesDetected++;
            }
        }
        if (numEnemiesDetected > 0)
        {
            hasDetectedEnemy = true;
        }
        else
        {
            hasDetectedEnemy = false;
        }
    }

    //Should be defined by subclasses.
    protected virtual void ReactToEnemy() { }
    public virtual void InitializeTroop() { }

    

    protected GameObject FindClosestThing(List<GameObject> thingList)
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

    //Used to add nearby buildings or enemies to lists.
    private void OnTriggerEnter(Collider other)
    {
        string otherTag = other.tag;
        if (enemyTags.Contains(otherTag) && !detectedObjects.Contains(other.gameObject))
        {
            Debug.Log(gameObject.name + " found enemy: " + other.gameObject.name);
            detectedObjects.Add(other.gameObject);
            movingTowardsPresetDestination = false;
        }
    }

    //Used to remove things that leave the detection radius.
    private void OnTriggerExit(Collider other)
    {
        if (detectedObjects.Contains(other.gameObject))
        {
            Debug.Log(gameObject.name + " lost enemy: " + other.gameObject.name);
            detectedObjects.Remove(other.gameObject);
        }
    }
}
