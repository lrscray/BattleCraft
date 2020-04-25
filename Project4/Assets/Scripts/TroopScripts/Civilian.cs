﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Civilian : MonoBehaviour
{
    //TODO: Change hiding method to use SetActive(false); and change manager/house they are assigned to, to set that active again.

    //list of points to patrol through
    public GameObject[] wanderPoints;
    //list of houses
    //public GameObject[] houses;

    private CivilianPopulation civilianManager;

    private BuildingManager civilianBuildingManager = null;

    private List<GameObject> civilianHouses;

    //Distance in which the Civilian locates their new point to wonder to
    //Solves issue with civilians bumping into each other at a point
    [SerializeField] private float minRemainingDistance = 0.5f;

    //private int destinationPoint = 0;
    private NavMeshAgent agent;
    bool hasADefender;
    bool inAHouse;

    int state = 1;

    void Start()
    {
        setInAHouse(false);
        //find all of the wander points when the object is created
        civilianManager = GameObject.FindGameObjectWithTag("CivilianManager").GetComponentInChildren<CivilianPopulation>();
        civilianBuildingManager = GameObject.FindGameObjectWithTag("CivilianBuildingManager").GetComponentInChildren<BuildingManager>();

        wanderPoints = GameObject.FindGameObjectsWithTag("CivilianWanderPoint");

        //houses = GameObject.FindGameObjectsWithTag("House");
        civilianHouses = civilianBuildingManager.GetAllBuildings();

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        int nextPoint = Random.Range(0, wanderPoints.Length);
        agent.destination = wanderPoints[nextPoint].transform.position;

        hasADefender = false;

    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Make sure this doesnt kill performance!
        civilianHouses = civilianBuildingManager.GetAllBuildings();

        if (state == 1)
            Wander();
        if (state == 2)
            Escort();
        //if (state == 3)
            //Hide();
        
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
        float closestHouseDist = Mathf.Infinity;//Vector3.Distance(transform.position, houses[0].transform.position);
        float nextDistance;
        int closestHouse = -1;
        for (int i = 0; i < civilianHouses.Count; i++)
        {
            if (civilianHouses[i] != null )//&& GetBuildingBehavior(i).HouseHasRoom()) //TODO: Think about enabling this! Would have to change defender behavior too. So they all dont go in the same house!
            {
                nextDistance = Vector3.Distance(transform.position, civilianHouses[i].transform.position);
                if (nextDistance < closestHouseDist)
                {
                    closestHouseDist = nextDistance;
                    closestHouse = i;
                }
            }
        }

        //move towards the closest civilian building.
        if (closestHouse != -1 && civilianHouses[closestHouse] != null)
        {
            transform.LookAt(civilianHouses[closestHouse].transform);
            GetComponent<Rigidbody>().AddForce(transform.forward * 10);
        }
    }
    //state 3
    void HideInBuilding(GameObject building)
    {
        building.GetComponentInChildren<BuildingBehavior>().StoreTroop(gameObject);
        setInAHouse(true);
        gameObject.SetActive(false);
    }

    private BuildingBehavior GetBuildingBehavior(int i)
    {
        return civilianHouses[i].GetComponentInChildren<BuildingBehavior>();
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
        if (collision.gameObject.tag == "Enemy2")
        {
            civilianManager.DestroyCivilian(gameObject);
            Destroy(gameObject);
        }
        //collision with house while in state 1. Change to state 2
        //TODO: Make it so that the civilians can only go in the civilian buildings?
        if (collision.gameObject.tag == "House" && state == 2)
        {
            state = 3;
            HideInBuilding(collision.gameObject);
        }
    }
    public void setHasADefender(bool val)
    {
        hasADefender = val; 
    }
    public bool getHasADefender() {
        return hasADefender;
    }
    public bool getInAHouse()
    {
        return inAHouse;
    }
    public void setInAHouse(bool val)
    {
        inAHouse = val;
    }

    public void SetState(int stateNum)
    {
        state = stateNum;
    }
}
