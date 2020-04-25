using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private string buildingManagerType = null; //What type of buildings are stored here?

    private List<GameObject> buildings;

    // Start is called before the first frame update
    void Start()
    {
        buildings = new List<GameObject>();
    }

    public List<GameObject> GetAllBuildings()
    {
        return buildings;
    }

    public void MakeBuilding(GameObject newBuilding)
    {
        buildings.Add(newBuilding);
    }

    public void DestroyBuilding(GameObject destroyedBuilding)
    {
        buildings.Remove(destroyedBuilding);
    }

    public string GetBuildingManagerType()
    {
        return buildingManagerType;
    }
}
