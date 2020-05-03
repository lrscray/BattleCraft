using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private string buildingManagerType = null; //What type of buildings are stored here?

    [SerializeField] private GameObject buildingTypePrefab = null; //Only added for object pooling.

    private List<GameObject> buildings;

    private void Awake()
    {
        buildings = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public List<GameObject> GetAllBuildings()
    {
        return buildings;
    }

    public void MakeBuilding(GameObject newBuilding)
    {
        //Debug.Log("NewBuilding: " + newBuilding);
        //Debug.Log("Buildings: " + buildings);
        buildings.Add(newBuilding);
    }

    public void DestroyBuilding(GameObject destroyedBuilding)
    {
        destroyedBuilding.SetActive(false);
        ObjectPoolManager.instance.DeactivateObject(buildingTypePrefab, destroyedBuilding);
        buildings.Remove(destroyedBuilding);
    }

    public string GetBuildingManagerType()
    {
        return buildingManagerType;
    }
}
