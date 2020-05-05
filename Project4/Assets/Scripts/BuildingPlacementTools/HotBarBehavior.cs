using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBarBehavior : MonoBehaviour
{
    [SerializeField] private PlayerBuildingToolSelectionBehavior buildingSelector = null;

    [SerializeField] private List<Button> hotBarButtons;

    public void SelectBuilding(int buttonKeyValue)
    {
        buildingSelector.SetCurrentSelectedBuilding(buttonKeyValue-1);
        buildingSelector.SetBuildingEnabled(true);
    }

    /*
    public void SelectBuilding1()
    {
        //Set playerbuilding tool selector to respond properly.
        buildingSelector.SetCurrentSelectedBuilding(0);
        buildingSelector.SetBuildingEnabled();
    }
    */

}
