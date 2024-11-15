using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public List<Transform> Waypoints = new List<Transform>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetWaypoints()
    {
        Transform[] wpList = transform.GetComponentsInChildren<Transform>();
        for(int i = 0; i < wpList.Length; i++)
        {
            if(wpList[i].CompareTag("Waypoint"))
            {
                Waypoints.Add(wpList[i]);
            }
        }
    }
}