using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceManager : MonoBehaviour
{
    [SerializeField] private int numResources = -1;

    [SerializeField] private Text numResourceLabel = null;

    // Start is called before the first frame update
    void Start()
    {
        numResources = 0;
        StartCoroutine(IncrementResourcesAfterTime());
    }

    IEnumerator IncrementResourcesAfterTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            numResources++;
            numResourceLabel.text = numResources.ToString();
        }
    }

    public int GetNumResources()
    {
        return numResources;
    }

    public void BuildBuilding(int buildingCost)
    {
        numResources -= buildingCost;
        numResourceLabel.text = numResources.ToString();
    }

    public void TakeUpKeep(int upKeepCost)
    {
        numResources -= upKeepCost;
        numResourceLabel.text = numResources.ToString();
    }

    
}
