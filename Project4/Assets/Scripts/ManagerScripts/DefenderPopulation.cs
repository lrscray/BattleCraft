using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderPopulation : MonoBehaviour
{
    //Store spawn locations for defenders
    [SerializeField] private GameObject[] DspawnLocations = null;
    //Sotres our new defender
    [SerializeField] private GameObject tempDefender = null;
    //Population size (Defenders)
    private int DpopSize;
    private int DcurrentPop;

    void Start()
    {
        DpopSize = 0;
        DpopSize = 0;
        SpawnDefenders();
    }


    public void SpawnDefenders()
    {
        while (DcurrentPop < DpopSize)
        {
            //update population
            DcurrentPop++;
            //Create a random spawn location for the new civilian
            int spawn = Random.Range(0, DspawnLocations.Length);
            //Instantiate
            GameObject troop = Instantiate(tempDefender, DspawnLocations[spawn].transform.position, Quaternion.identity);
            troop.transform.SetParent(transform);
        }
    }
}
