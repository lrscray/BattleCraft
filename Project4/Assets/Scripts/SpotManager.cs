﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//cant use navmesh without this
using UnityEngine.AI;

public class SpotManager : MonoBehaviour
{

    public bool isOccupied;
    public bool notOccupied;
    GameObject Collector;
    // Start is called before the first frame update
    void Start()
    {
        Collector = GameObject.FindGameObjectWithTag("Collector");
        isOccupied = false;
        
    }

    public void Check()
    {
        if (isOccupied)
        {
            notOccupied = false;
        }
        else
        {
            notOccupied = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {

        //Check if Collector is colliding with a Mining spot
        if (other.CompareTag("Collector"))
        {
            isOccupied = true;   
        }
        else
        {
            isOccupied = false;
        }

        Check();
    }

    public void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Collector"))
        {
            gameObject.SetActive(false);
        }
    }

}
