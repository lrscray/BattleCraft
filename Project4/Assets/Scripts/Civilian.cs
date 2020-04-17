using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Civilian : MonoBehaviour
{
    //TODO: Need to implement health, and decrementing from the CivilianManager.
    //TODO: Change hiding method to use SetActive(false); and change manager/house they are assigned to, to set that active again.

    //list of points to patrol through
    public GameObject[] wanderPoints;
    //list of houses
    public GameObject[] houses;

    //Distance in which the Civilian locates their new point to wonder to
    //Solves issue with civilians bumping into each other at a point
    [SerializeField] private float minRemainingDistance = 0.5f;

    private int destinationPoint = 0;
    private NavMeshAgent agent;
    bool hasADefender;

    int state = 1;

    void Start()
    {
        //find all of the wander points when the object is created
        wanderPoints = GameObject.FindGameObjectsWithTag("CivilianWanderPoint");
        houses = GameObject.FindGameObjectsWithTag("House");

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        int nextPoint = Random.Range(0, wanderPoints.Length);
        agent.destination = wanderPoints[nextPoint].transform.position;

        hasADefender = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(state == 1)
            Wander();
        if (state == 2)
            Escort();
        if (state == 3)
            Hide();
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
    }
    //state 2
    void Escort()
    {
        //locate nearest house to go to
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
        GetComponent<Rigidbody>().AddForce(transform.forward * 5);
        //Once contact is made with the citizen. move them to the nearest house
    }
    //state 3
    void Hide()
    {
        //GetComponent(MeshRenderer).enabled = false;
        //renderer.enabled = false;
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {

        //collision into Defender while defender is in state 2 or 3. Change state to 1.
        if (collision.gameObject.tag == "Defender")
        {
            GameObject theDefender = collision.gameObject;
            if (theDefender.GetComponent<Defender>().getState() == 2 || theDefender.GetComponent<Defender>().getState() == 3)
            {
                setHasADefender(true);
                state = 2;
            }
        }

        //collision with house while in state 1. Change to state 2
        if (collision.gameObject.tag == "House" && state == 2)
        {
            state = 3;
        }
    }
    public void setHasADefender(bool val)
    {
        hasADefender = val; 
    }
    public bool getHasADefender() {
        return hasADefender;
    }
}
