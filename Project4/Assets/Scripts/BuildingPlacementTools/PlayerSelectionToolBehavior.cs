using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionToolBehavior : MonoBehaviour
{
    static PlayerSelectionToolBehavior _instance;

    public static PlayerSelectionToolBehavior instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerSelectionToolBehavior>();
            }
            return _instance;
        }
    }

    [SerializeField] private KeyCode primarySelectKey = 0;
    [SerializeField] private KeyCode secondaryUnSelectKey = 0;

    [SerializeField] private Camera mainCamera = null;

    //Should only be enabled if not currently trying to place a building.
    private bool selectingEnabled = true;

    //TODO: Is this needed?
    private GameObject selectedObject = null;



    // Update is called once per frame
    void Update()
    {
        if (selectingEnabled == true)
        {
            CheckPrimaryInput();
        }
        if (Input.GetKeyDown(secondaryUnSelectKey))
        {
            //If any menus are open, close them.
            BuildingSelectionPanelBehavior.instance.CloseMenu();
            selectingEnabled = true;
            selectedObject = null;
        }
    }

    private void CheckPrimaryInput()
    {
        //If clicked on collider of building or troop, select them. 
        //If building, open building selection menu.
        if (Input.GetKeyDown(primarySelectKey))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); //Create a raycast to that point from camera.
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) //If hit something with a collider.
            {
                //Debug.Log(hit.collider.gameObject.name);
                GameObject clickedObject = hit.collider.gameObject;

                //If clicked on building.
                if (clickedObject.GetComponentInChildren<BuildingBehavior>() != null)
                {
                    PlayerBuildingToolSelectionBehavior.instance.SetBuildingPlacementSelectionEnabled(false);
                    selectedObject = clickedObject;
                    BuildingSelectionPanelBehavior.instance.SetupOpenMenu(clickedObject);
                    selectingEnabled = false;
                }
            }
        }
    }

    public void SetSelectingEnabled(bool newValue)
    {
        selectingEnabled = newValue;
    }

    public GameObject GetSelectedObject()
    {
        return selectedObject;
    }
}
