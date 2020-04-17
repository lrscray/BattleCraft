using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderPopulation : MonoBehaviour
{
    //Store spawn locations for defenders
    public GameObject[] DspawnLocations;
    //Sotres our new defender
    public GameObject tempDefender;
    //Population size (Defenders)
    int DpopSize;
    int DcurrentPop;

    void Start()
    {
        DpopSize = 0;
        DpopSize = 5;
        SpawnDefenders();
    }
    public void SpawnDefenders()
    {
        while (DcurrentPop < DpopSize)
        {
            //update population
            DcurrentPop++;
            //find the prefab in resources
            tempDefender = (GameObject)Resources.Load("Defender", typeof(GameObject));
            //Create a random spawn location for the new civilian
            int spawn = Random.Range(0, DspawnLocations.Length);
            //Instantiate
            GameObject.Instantiate(tempDefender, DspawnLocations[spawn].transform.position, Quaternion.identity);
        }
    }
}
