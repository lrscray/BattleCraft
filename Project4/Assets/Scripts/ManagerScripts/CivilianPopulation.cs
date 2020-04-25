﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class spawns civilians
public class CivilianPopulation : MonoBehaviour
{
    //Store spawn locations for civilians
    [SerializeField] private GameObject[] spawnLocations = null;
    //Stores our new civilian
    [SerializeField] private GameObject tempCivilian = null;

    //Population size. This acts as the health for the base (Civilian)
    private int maxPopSize;
    [SerializeField] private int numCurrentCivilians = 0;

    [SerializeField] private int numStartingCivilians = 5;

    [SerializeField] private HealthBarScript healthBar = null;

    private void Start()
    {
        SpawnStartingCivilians();
        healthBar.SetMaxHealth(numStartingCivilians);
    }

    public void SpawnStartingCivilians()
    {
        for(int i = 0; i < numStartingCivilians; i++)
        {
            //update population
            numCurrentCivilians++;
            //find the prefab in resources
            //tempCivilian = (GameObject)Resources.Load("Civilian", typeof(GameObject));
            //Create a random spawn location for the new civilian
            int spawn = Random.Range(0, spawnLocations.Length);
            //Instantiate
            GameObject troop = Instantiate(tempCivilian, spawnLocations[spawn].transform.position, Quaternion.identity);
            troop.transform.SetParent(transform);
        }
    }

    public void IncrementNumCurrentCivilians()
    {
        numCurrentCivilians++;
        numStartingCivilians++;//TODO: Consider changing how this works.
        healthBar.SetMaxHealth(numStartingCivilians + 1);
        healthBar.SetHealth(GetNumCurrentCivilians());
    }

    public int GetNumCurrentCivilians()
    {
        return numCurrentCivilians;
    }

    public void DecrementNumCurrentCivilians()
    {
        numCurrentCivilians--;
        healthBar.SetHealth(GetNumCurrentCivilians());
    }
}