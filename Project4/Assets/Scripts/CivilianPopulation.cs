using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class spawns civilians
public class CivilianPopulation : MonoBehaviour
{
    //Store spawn locations for civilians
    public GameObject[] spawnLocations;
    //Stores our new civilian
    public GameObject tempCivilian;

    //Population size. This acts as the health for the base (Civilian)
    private int maxPopSize;
    [SerializeField] private int numCurrentCivilians = 0;

    [SerializeField] private int numStartingCivilians = 5;

    private void Start()
    {
        SpawnStartingCivilians();
    }

    public void SpawnStartingCivilians()
    {
        for(int i = 0; i < numStartingCivilians; i++)
        {
            //update population
            numCurrentCivilians++;
            //find the prefab in resources
            tempCivilian = (GameObject)Resources.Load("Civilian", typeof(GameObject));
            //Create a random spawn location for the new civilian
            int spawn = Random.Range(0, spawnLocations.Length);
            //Instantiate
            GameObject.Instantiate(tempCivilian, spawnLocations[spawn].transform.position, Quaternion.identity);
        }
    }

    public int GetNumCurrentCivilians()
    {
        return numCurrentCivilians;
    }

    public void DecrementNumCurrentCivilians()
    {
        numCurrentCivilians--;
    }
}
