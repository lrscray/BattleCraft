using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Troop : MonoBehaviour
{

    [SerializeField] private NavMeshAgent navAgent = null;

    [SerializeField] private bool isPointsPatrolType = true; //If true, the enemy patrols using 4 points in the map. Else, will use a patrol circle radius.
    [SerializeField] private int patrolCircleRadius = 5; //Used if isPointsPatrolType is false.

    [SerializeField] private List<Transform> patrolList = null;

    private bool detectedEnemy = false; //If this troop detects what it considers to be an enemy.

    [SerializeField] private int detectionRadius = 20; //TODO: Consider using OnTriggerEnter...

    [SerializeField] private int chaseOffset = 2; //The distance this troop will stand from things it chases.

    [SerializeField] private int attackRadius = 2; //The radius that enemies must be in to be attacked by this troop.

    private bool isSelected = false; //Whether or not the player has selected this troop.

    // Start is called before the first frame update
    void Start()
    {
        //patrolList = new List<Transform>(); //Needed?
    }

    protected virtual void ReactToEnemy() { }
}
