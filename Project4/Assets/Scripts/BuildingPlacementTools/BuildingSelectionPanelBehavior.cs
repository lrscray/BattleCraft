using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectionPanelBehavior : MonoBehaviour
{
    static BuildingSelectionPanelBehavior _instance;

    public static BuildingSelectionPanelBehavior instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<BuildingSelectionPanelBehavior>();
            }
            return _instance;
        }
    }

    [SerializeField] private GameObject buildingSelectionPanel = null;

    [SerializeField] private Text buildingNameText = null;
    [SerializeField] private Text troopCreationCostText = null;
    [SerializeField] private Text numTroopsToSpawnText = null;

    private BuildingBehavior selectedBuilding = null;

    //TODO: PERF: Find a way of updating this text without using update to save frames.
    private void Update()
    {
        //If menu is open,
        if(selectedBuilding != null)
        {
            SetNumTroopsToSpawnText(selectedBuilding.GetNumTroopsToSpawn());
        }
    }

    public void SetupOpenMenu(GameObject selectedBuildingGameObject)
    {
        selectedBuilding = selectedBuildingGameObject.GetComponentInChildren<BuildingBehavior>();
        //Change texts of menu.
        buildingNameText.text = selectedBuilding.name.ToString();
        troopCreationCostText.text = selectedBuilding.GetTroopCreationCost().ToString();

        SetNumTroopsToSpawnText(selectedBuilding.GetNumTroopsToSpawn());

        //Then open menu.
        buildingSelectionPanel.SetActive(true);
    }

    public void SetNumTroopsToSpawnText(int newNumTroopsToSpawn)
    {
        numTroopsToSpawnText.text = newNumTroopsToSpawn.ToString();
    }

    public void IncrementNumTroopsToMake()
    {
        selectedBuilding.IncrementNumTroopsToSpawn();
        SetNumTroopsToSpawnText(selectedBuilding.GetNumTroopsToSpawn());
        selectedBuilding.StartSpawningTroops();
    }

    public void DecrementNumTroopsToMake()
    {
        selectedBuilding.DecrementNumTroopsToSpawn();
        SetNumTroopsToSpawnText(selectedBuilding.GetNumTroopsToSpawn());
    }

    public GameObject GetSelectedBuildingGameObject()
    {
        return selectedBuilding.gameObject;
    }

    public void CloseMenu()
    {
        PlayerSelectionToolBehavior.instance.SetSelectingEnabled(true);
        PlayerBuildingToolSelectionBehavior.instance.SetBuildingPlacementSelectionEnabled(true);
        buildingSelectionPanel.SetActive(false);
    }
}
