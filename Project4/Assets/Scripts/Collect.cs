using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//cant use navmesh without this
using UnityEngine.AI;

public class Collect : MonoBehaviour
{
    GameObject target;
    GameObject home;
    //keep track of whether collector is collecting or depositting
    [SerializeField] private GameObject CollectionCheck;
    [SerializeField] private GameObject MineSpot1;
    [SerializeField] private GameObject MineSpot2;
    [SerializeField] private GameObject MineSpot3;
    [SerializeField] private GameObject MineSpot4;
    /*[SerializeField] private GameObject MineSpot5;
    [SerializeField] private GameObject MineSpot6;
    [SerializeField] private GameObject MineSpot7;
    [SerializeField] private GameObject MineSpot8;*/
    [SerializeField] private bool isCollecting;
    [SerializeField] private bool isDepositing;
    [SerializeField] private float startWait = 5.0f;
    [SerializeField] private float Storage;
    [SerializeField] private float bePatient;
    [SerializeField] private float Inventory;
    [SerializeField] private int inactive;

    private NavMeshAgent agent;
    private float waitTime;

    //Allow access to other script
    public SpotManager spotmanager;

    //Get the Navmesh agent and Mining spots at the start, set waitTime
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("MiningSpot");
        home = GameObject.FindGameObjectWithTag("Deposite");
        isCollecting = true;
        isDepositing = false;
        Storage = 0;
        Inventory = 0;
        bePatient = 2.0f;
        inactive = 0;
        waitTime = startWait;
        //Check this function in the other script
        spotmanager.Check();
    }

    void Update()
    {

        ClosestMiningSpot();
        //Check if too many spots are inactive and refresh all spots
        if (inactive >= 2)
        {
            Refresh();
            //reset inactive
            inactive = 0;
        }
     
    }

    //This code is NOT running in the program.
    //Tell Collectors to go and collect crystals
    private void goCollect()
    {
        if(isCollecting && spotmanager.notOccupied)
        {
            agent.SetDestination(target.transform.position);
        }
        
    }

    //This code is NOT running in the program.
    //Tell Collectors to go deposite their crsytals
    private void goDeposite()
    {
        if(isDepositing && spotmanager.notOccupied)
        {
            agent.SetDestination(home.transform.position);
        }

    }

    //Have collector trigger something after colliding with a mining spot
    private void OnTriggerStay(Collider other)
    {
        
        //Checks if Collector is colliding with a Mining spot
        if (other.CompareTag("MiningSpot"))
        {
            StartCoroutine(Collected());
            
        }

        //Checks if Collector is colliding with a Home
        if (other.CompareTag("Deposite"))
        {
            StartCoroutine(Deposited());
            StartCoroutine(Reactivate());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MiningSpot"))
        {
            //increase inventory by 5
            Inventory += 5;
            inactive += 1;
        }

        if (other.CompareTag("Deposite"))
        {
            //Add Inventory to Storage and decrease Inventory
            Storage += Inventory;
            Inventory = 0;
        }
         
    }

    private void OnTriggerEnter(Collider other)
    {
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
            float distanceTospot = (currentSpot.transform.position - this.transform.position).sqrMagnitude;
            if (distanceTospot < distanceToClosestspot)
            {

                distanceToClosestspot = distanceTospot;
                closestSpot = currentSpot;

                if (isCollecting)
                {
                    agent.SetDestination(currentSpot.transform.position);
                }

            }
                     
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

                if (isDepositing)
                {
                    agent.SetDestination(currentSpot.transform.position);
                }
                
            }

        }
        Debug.DrawLine(this.transform.position, closestSpot.transform.position);
        return;

    }

    void Refresh()
    {
        MineSpot1.SetActive(true);
        MineSpot2.SetActive(true);
        MineSpot3.SetActive(true);
        MineSpot4.SetActive(true);
       /* MineSpot5.SetActive(true);
        MineSpot6.SetActive(true);
        MineSpot7.SetActive(true);
        MineSpot8.SetActive(true);*/
    }

    IEnumerator Collected()
    {
        //Wait 5 seconds!
        yield return new WaitForSeconds(3);
        //Make object above collectors visible
        CollectionCheck.SetActive(true);
        isCollecting = false;
        isDepositing = true;
        ClosestDepositeSpot();
        //goDeposite();
       
    }

    IEnumerator Deposited()
    {
        yield return new WaitForSeconds(3);
        CollectionCheck.SetActive(false);
        isCollecting = true;
        isDepositing = false;
        ClosestMiningSpot();
        //goCollect();
    }

    IEnumerator JustWait()
    {
        yield return new WaitForSeconds(bePatient);
    }

    IEnumerator Reactivate()
    {
        if (target.activeInHierarchy)
        {
            ClosestDepositeSpot();
        }

        yield return null;
    }

}