using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollector : MonoBehaviour
{
    [SerializeField] private GameObject Collectors = null;
    [SerializeField] private float Delay = 5.0f;
    [SerializeField] private GameObject CollectionCheck = null;
    //[SerializeField] private bool stopSpawn = false;

    //set limit for how many will be spawned
    [SerializeField] private int cap = 5;
    private int householdsize = 0;
    private float Wait;

    void Start()
    {

    }

    void Update()
    {
        //Want there to be a delay before the next collector spawns
        
        if (householdsize < cap && Lapse())
        {
            CollectionCheck.SetActive(false);
            SpawnIt();
            Count();
        }

    }
    //This will keep track of the house hold. 
    void Count()
    {
        householdsize++;
    }

    void SpawnIt()
    {
        Wait = Time.time + Delay;
        Instantiate(Collectors, transform.position, Quaternion.identity);
        
    }

    private bool Lapse()
    {
        return Time.time >= Wait;
    }

}
