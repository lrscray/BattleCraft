using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingToolPlacingBehavior : MonoBehaviour
{
    static PlayerBuildingToolPlacingBehavior _instance;

    public static PlayerBuildingToolPlacingBehavior instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerBuildingToolPlacingBehavior>();
            }
            return _instance;
        }
    }

    //The key used to place buildings. Probably left mouse button.
    [SerializeField] private KeyCode placingKey = 0;

    [SerializeField] private GameObject worldUp = null;

    private GameObject ghostObject;
    private bool ghostObjectAlreadyCreated;
    private bool placingEnabled;
    private GameObject prevSelectedGhostBuilding = null;

    //[SerializeField] private Transform createdBuildingsFolder = null;

    private void Update()
    {
        if(PlayerBuildingToolSelectionBehavior.instance.IsBuildingEnabled())
        {
            //Debug.Log("Attempting to create ghost object...");
            CreateUpdateGhost();
            
            CheckPlaceButtonPressed();
        }
        else
        {
            CallGhostBuster();
        }
    }

    private void CheckPlaceButtonPressed()
    {
        if (Input.GetKeyDown(placingKey))
        {
            //Check if building is in mouse positions spot/area.
            if (placingEnabled == true)
            {
                if (PlayerResourceManager.instance.GetNumResources() >= PlayerBuildingToolSelectionBehavior.instance.GetCurrentSelectedBuilding().GetComponentInChildren<BuildingBehavior>().GetBuildingCreationCost())
                {
                    //Spawn building.
                    PlayerResourceManager.instance.BuildBuilding(PlayerBuildingToolSelectionBehavior.instance.GetCurrentSelectedBuilding().GetComponentInChildren<BuildingBehavior>().GetBuildingCreationCost());
                    //GameObject building = Instantiate(buildingToolSelector.GetCurrentSelectedBuilding(), ghostObject.transform.position, ghostObject.transform.rotation);
                    ObjectPoolManager.instance.GetNextObject(PlayerBuildingToolSelectionBehavior.instance.GetCurrentSelectedBuilding(), ghostObject.transform.position, ghostObject.transform.rotation);

                    placingEnabled = false;
                    CallGhostBuster();
                    PlayerBuildingToolSelectionBehavior.instance.PlaceBuilding();

                    //update the navMesh when a building is spawned
                    NavMeshManager.instance.UpdateNavMesh();
                }
            }
        }
    }

    private void CreateUpdateGhost()
    {
        Ray rayPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(rayPoint, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            //Create ghost version of building to place to use to check if theres another building in placing location.
            if (ghostObjectAlreadyCreated == false)
            {
                //Debug.Log("Attempting to instantiate ghost...");
                //Instantiate(buildingToolSelector.GetCurrentSelectedGhostBuilding(), hit.point, worldUp.transform.rotation);
                ghostObject = ObjectPoolManager.instance.GetNextObject(PlayerBuildingToolSelectionBehavior.instance.GetCurrentSelectedGhostBuilding(), hit.point, worldUp.transform.rotation);
                prevSelectedGhostBuilding = PlayerBuildingToolSelectionBehavior.instance.GetCurrentSelectedGhostBuilding();
                //Debug.Log("Instantiated ghost!");
                ghostObjectAlreadyCreated = true;
            }

            ghostObject.transform.position = hit.point;

            if (ghostObject.GetComponentInChildren<BuildingGhostBehavior>().IsCollidingWBuilding() == true)
            {
                //Debug.Log("Cannot place here!");
                placingEnabled = false;
            }
            else
            {
                //Debug.Log("Can place here!");
                placingEnabled = true;
            }
        }
    }

    //Cleans up unwanted ghost object.
    private void CallGhostBuster()
    {
        if (ghostObject != null)
        {
            //Destroy(ghostObject);
            ghostObject.SetActive(false);
            ObjectPoolManager.instance.DeactivateObject(prevSelectedGhostBuilding, ghostObject);
            ghostObject = null;
            ghostObjectAlreadyCreated = false;
        }
    }
}
