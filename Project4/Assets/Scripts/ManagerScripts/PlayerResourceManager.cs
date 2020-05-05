using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceManager : MonoBehaviour
{
    static PlayerResourceManager _instance;

    public static PlayerResourceManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerResourceManager>();
            }
            return _instance;
        }
    }

    [SerializeField] private bool inDebugMode = false;

    [SerializeField] private int numResources = -1;

    [SerializeField] private Text numResourceLabel = null;

    // Start is called before the first frame update
    void Start()
    {
        if (!inDebugMode)
        {
            numResources = 0;
        }
        else
        {
            numResources = 100000;
        }
        //StartCoroutine(IncrementResourcesAfterTime());
    }
    /*
    IEnumerator IncrementResourcesAfterTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            numResources++;
            numResourceLabel.text = numResources.ToString();
        }
    }
    */
    public int GetNumResources()
    {
        return numResources;
    }

    public void IncrementNumResources(int resourceAmmount)
    {
        numResources += resourceAmmount;
        numResourceLabel.text = numResources.ToString();
    }

    public void BuildBuilding(int buildingCost)
    {
        numResources -= buildingCost;
        numResourceLabel.text = numResources.ToString();
    }

    public void UseResources(int amountOfResources)
    {
        numResources -= amountOfResources;
        numResourceLabel.text = numResources.ToString();
    }

    public void TakeUpKeep(int upKeepCost)
    {
        numResources -= upKeepCost;
        numResourceLabel.text = numResources.ToString();
    }

    
}
