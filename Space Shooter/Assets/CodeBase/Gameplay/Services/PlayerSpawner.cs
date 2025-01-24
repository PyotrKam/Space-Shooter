using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private FollowCamera m_FollowCameraPrefab;
        [SerializeField] private Player m_PlayerPrefab;
        [SerializeField] private ShipInputController m_ShipIpuntControllerPrefab;
        [SerializeField] private VirtualGamePad m_VirtualGamePadPrefab;

        [SerializeField] private Transform m_SpawPoint;

        private void Start()
        {
            FollowCamera followCamera = Instantiate(m_FollowCameraPrefab);
            VirtualGamePad virtualGamePad = Instantiate(m_VirtualGamePadPrefab);

            ShipInputController shipInputController = Instantiate(m_ShipIpuntControllerPrefab);
            shipInputController.Construct(virtualGamePad);

            Player player = Instantiate(m_PlayerPrefab);
            player.Construct(followCamera, shipInputController, m_SpawPoint);
        }

    }
}

    
