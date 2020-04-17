using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingToolSelectionBehavior : MonoBehaviour
{
    private bool buildingEnabled = false;
    private GameObject currentSelectedBuilding = null; //The building type to be built.
    
    //Fill this list with all building prefabs.
    [SerializeField] private List<GameObject> buildingTypePrefabs = null;
    [SerializeField] private List<GameObject> buildingTypeGhostPrefabs = null;
    //Fill this list with all keys for selecting the appropriate(same index) building type.
    [SerializeField] private List<KeyCode> buildingTypeKeyCodeSelectors = null;
    [SerializeField] private KeyCode unSelectBuildingKey;

    //TODO - PERF IMPR: Consider another way of not running through each possible key selector to check if placed.
    private void Update()
    {
        for (int i = 0; i < buildingTypeKeyCodeSelectors.Count; i++)
        { 
            if (Input.GetKeyDown(buildingTypeKeyCodeSelectors[i])) //If player pressed input button/key for that building type.
            {
                currentSelectedBuilding = buildingTypePrefabs[i];
                //Debug.Log("Pressed button: " + buildingTypeKeyCodeSelectors[i]);
                buildingEnabled = true;
            }
        }
        if(Input.GetKeyDown(unSelectBuildingKey)) //If the player clicked the button for unselecting the build tool.
        {
            currentSelectedBuilding = null;
            //Debug.Log("Pressed unselect button.");
            buildingEnabled = false;
        }   
    }

    public GameObject GetCurrentSelectedBuilding()
    {
        return currentSelectedBuilding;
    }

    public GameObject GetCurrentSelectedGhostBuilding()
    {
        return buildingTypeGhostPrefabs[buildingTypePrefabs.IndexOf(currentSelectedBuilding)];
    }
    
    public bool IsBuildingEnabled()
    {
        return buildingEnabled;
    }

    //Deselects the building tool after an object is placed.
    public void PlaceBuilding()
    {
        buildingEnabled = false;
        currentSelectedBuilding = null;
    }
}
