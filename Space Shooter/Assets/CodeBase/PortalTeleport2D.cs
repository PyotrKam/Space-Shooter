using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport2D : MonoBehaviour
{
    [SerializeField] private Transform destinationPortal; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Get the Rigidbody2D component from the object that entered the trigger
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        // If Rigidbody2D exists, move it
        if (rb != null)
        {
            rb.position = destinationPortal.position;
        }
        // If there is no Rigidbody2D, move the object using transform
        else
        {
            other.transform.position = destinationPortal.position;
        }
    }
}
