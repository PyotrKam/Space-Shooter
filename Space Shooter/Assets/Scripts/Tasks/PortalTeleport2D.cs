using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport2D : MonoBehaviour
{
    [SerializeField] private Transform destinationPortal;
    [SerializeField] private Transform m_player;
    [SerializeField] private Transform m_camera;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        m_player.position = destinationPortal.position;

        m_camera.position = new Vector3(destinationPortal.position.x, destinationPortal.position.y, m_camera.position.z);
    }
}
