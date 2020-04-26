using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface surface;


    // Start is called before the first frame update
    void Start()
    {
        surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
     //  surface.BuildNavMesh();
    }

    public void UpdateNavMesh()
    {
        surface.BuildNavMesh();
       print("updated mesh");
    }
}
