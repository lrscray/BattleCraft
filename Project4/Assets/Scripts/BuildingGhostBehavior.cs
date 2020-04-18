using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhostBehavior : MonoBehaviour
{
    private bool isCollidingWBuilding = false; //Whether the building is currently colliding with another building. For placing purposes.

    //Consider using OnTriggerEnter if having collision issues.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Building")
        {
            Debug.Log("Colliding!");
            if (isCollidingWBuilding == false)
            {
                isCollidingWBuilding = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (isCollidingWBuilding == true)
        {
            isCollidingWBuilding = false;
        }
    }

    public bool IsCollidingWBuilding()
    {
        return isCollidingWBuilding;
    }
}
