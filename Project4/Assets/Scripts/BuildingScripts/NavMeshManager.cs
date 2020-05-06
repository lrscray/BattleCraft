using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    static NavMeshManager _instance;

    public static NavMeshManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<NavMeshManager>();
            }
            return _instance;
        }
    }

    public NavMeshSurface surface;


    // Start is called before the first frame update
    void Start()
    {
        surface.BuildNavMesh();
    }

    public void UpdateNavMesh()
    {
        surface.UpdateNavMesh(surface.navMeshData);
        //print("updated mesh");
    }
}
