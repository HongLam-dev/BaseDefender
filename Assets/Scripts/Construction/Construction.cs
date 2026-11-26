using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Construction : MonoBehaviour
{
    [SerializeField] protected int teamID=0;

    virtual protected void Start()
    {
        GetComponent<Health>().TeamID = teamID;
    }

    virtual protected void Update()
    {
        
    }

}
