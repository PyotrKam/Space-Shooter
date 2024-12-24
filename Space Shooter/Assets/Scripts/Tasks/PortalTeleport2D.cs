using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport2D : MonoBehaviour
{
    [SerializeField] private Transform destinationPortal;
    



    private void OnTriggerEnter2D(Collider2D other)
    {


        other.transform.position = destinationPortal.position;

       
    }
}
