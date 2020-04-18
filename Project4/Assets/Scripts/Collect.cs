using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//cant use navmesh without this
using UnityEngine.AI;

public class Collect : MonoBehaviour
{
    private GameObject target;
    GameObject home;
    //keep track of whether collector is collecting or depositting
    [SerializeField] private GameObject CollectionCheck;
    [SerializeField] private bool isCollecting;
    [SerializeField] private bool isDepositing;
    private bool isWaiting = false;
    private bool isAtDestination = false;

    [SerializeField] private float startWait = 5.0f;
    private PlayerResourceManager resourceManager = null;

    [SerializeField] private float bePatient;
    [SerializeField] private int maxNumResourcesCarryable = 5;
    [SerializeField] private int numResourcesCarrying;
    [SerializeField] private int numResourcesPerCollect = 5;

    private NavMeshAgent agent;
    private float waitTime;
    private SpotManager spotmanager;

    //Get the Navmesh agent and Mining spots at the start, set waitTime
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        home = GameObject.FindGameObjectWithTag("Deposite");
        spotmanager = home.GetComponentInChildren<SpotManager>();
        resourceManager = GameObject.FindGameObjectWithTag("ResourceManager").GetComponentInChildren<PlayerResourceManager>();

        isCollecting = true;
        isDepositing = false;
        numResourcesCarrying = 0;
        bePatient = 2.0f;
        waitTime = startWait;
    }

    void Update()
    {
        if((isCollecting == true) && (isAtDestination == false))
        {
            ClosestMiningSpot();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MiningSpot"))// && (other == target))
        {
            isAtDestination = false;
        }         
    }

    private void OnTriggerEnter(Collider other)
    {
        //Checks if Collector is colliding with a Mining spot
        if (other.CompareTag("MiningSpot")) // && (other == target))
        {
            StartCoroutine(Collected());
            isAtDestination = true;
        }

        //Checks if Collector is colliding with a Home
        if (other.CompareTag("Deposite"))
        {
            StartCoroutine(Deposited());
        }
        if (other.CompareTag("Collector"))
        {
            StartCoroutine(JustWait());
        }
    }

    //Allow the collectors to choose the closes spot to them
    void ClosestMiningSpot()
    {
        float distanceToClosestspot = Mathf.Infinity;
        MSpots closestSpot = null;
        MSpots[] allSpots = GameObject.FindObjectsOfType<MSpots>();
        
        foreach (MSpots currentSpot in allSpots)
        {
            spotmanager = currentSpot.GetComponentInChildren<SpotManager>();
            float distanceTospot = (currentSpot.transform.position - this.transform.position).sqrMagnitude;
            if ((distanceTospot < distanceToClosestspot) && (spotmanager.GetIsOccupied() == false))
            {

                distanceToClosestspot = distanceTospot;
                closestSpot = currentSpot;

                //target = currentSpot.gameObject;
                agent.SetDestination(currentSpot.transform.position);
            }
                     
        }
        if(closestSpot == null)
        {
            StartCoroutine(JustWait());
        }
        Debug.DrawLine(this.transform.position, closestSpot.transform.position);
        return;

    }

    //Allow the collectors to choose the closest deposite spot to them
    void ClosestDepositeSpot()
    {
        float distanceToClosestspot = Mathf.Infinity;
        DSpots closestSpot = null;
        DSpots[] allSpots = GameObject.FindObjectsOfType<DSpots>();

        foreach (DSpots currentSpot in allSpots)
        {
           
            float distanceTospot = (currentSpot.transform.position - this.transform.position).sqrMagnitude;

            if (distanceTospot < distanceToClosestspot)
            {
                distanceToClosestspot = distanceTospot;
                closestSpot = currentSpot;

                agent.SetDestination(currentSpot.transform.position);
            }
        }
        if(closestSpot == null)
        {
            StartCoroutine(JustWait());
        }

        Debug.DrawLine(this.transform.position, closestSpot.transform.position);
        return;

    }

    IEnumerator Collected()
    {
        //Wait 5 seconds!
        yield return new WaitForSeconds(3);
        if(numResourcesCarrying <= maxNumResourcesCarryable)
        {
            if((maxNumResourcesCarryable - numResourcesCarrying) < numResourcesPerCollect)
            {
                numResourcesCarrying = maxNumResourcesCarryable;
            }
            else
            {
                numResourcesCarrying += numResourcesPerCollect;
            }
            
        }
        //Make object above collectors visible
        CollectionCheck.SetActive(true);
        isCollecting = false;
        isDepositing = true;
        ClosestDepositeSpot();       
    }

    IEnumerator Deposited()
    {
        yield return new WaitForSeconds(3);
        resourceManager.IncrementNumResources(numResourcesCarrying);
        numResourcesCarrying = 0;
        CollectionCheck.SetActive(false);
        isCollecting = true;
        isDepositing = false;
        ClosestMiningSpot();
    }

    IEnumerator JustWait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(bePatient);
        isWaiting = false;
    }
}