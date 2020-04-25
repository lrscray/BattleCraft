using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhostBehavior : MonoBehaviour
{
    private bool isCollidingWBuilding = false; //Whether the building is currently colliding with another building. For placing purposes.

    private bool OtherHasBuildingTags(string otherTag)
    {
        if (otherTag == "Building" || otherTag == "House" || otherTag == "Home" || otherTag == "MiningSpot" || otherTag == "Crystal" || otherTag == "Deposite")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: Should probably get consistent with our tags to avoid confusion.
        if (OtherHasBuildingTags(other.transform.tag))
        {
            //Debug.Log("Colliding!");
            isCollidingWBuilding = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (OtherHasBuildingTags(other.transform.tag))
        {
            //Debug.Log("Colliding!");
            isCollidingWBuilding = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isCollidingWBuilding = false;
    }

    public bool IsCollidingWBuilding()
    {
        return isCollidingWBuilding;
    }
}
