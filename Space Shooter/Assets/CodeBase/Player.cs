using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {

        public static SpaceShip SelectedSpaceShip;

        [SerializeField] private int m_NumLives;
        
        [SerializeField] private SpaceShip m_PlayerShipPrefab;
        public SpaceShip ActiveShip => m_Ship;

        [SerializeField] private CameraController m_CameraController;
        [SerializeField] private MovementCntroller m_MovementController;

        private SpaceShip m_Ship;

        private int m_Score;
        private int m_NumKills;

        public int Score => m_Score;
        public int NumKills => m_NumKills;
        public int NumLives => m_NumLives;

        public SpaceShip ShipPrefab 
        {
            get 
            {
                if (SelectedSpaceShip == null)
                {
                    return m_PlayerShipPrefab;
                }
                else
                {
                    return SelectedSpaceShip;
                }
            
            }
        
        }

        private void Start()
        {
            Respawn();
        }
        private void OnShipDeath()
        {
            m_NumLives--;

            if (m_NumLives > 0)
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            var newPlayerShip = Instantiate(ShipPrefab);

            m_Ship = newPlayerShip.GetComponent<SpaceShip>();

            m_Ship.EventOnDeath.AddListener(OnShipDeath);

            m_CameraController.SetTarget(m_Ship.transform);
            m_MovementController.SetTargetShip(m_Ship);

        }

        public void AddKill()
        {
            m_NumKills += 1;
        }

        public void AddScore(int num)
        {
            m_Score += num;
        }


    }
}

