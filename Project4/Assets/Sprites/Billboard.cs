using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public GameObject[] theCam;

    void Start()
    {
        theCam = GameObject.FindGameObjectsWithTag("MainCamera");
    }
 
    void LateUpdate()
    {
        transform.LookAt(transform.position + theCam[0].transform.position);
    }
}
