using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SpaceShooter
{
    public class ShipSelectionButton : MonoBehaviour
    {
        [SerializeField] private MainMenu m_MainMenu;
        [SerializeField] private SpaceShip m_Prefab;
        [SerializeField] private Text m_ShipName;
        [SerializeField] private Text m_Hitpoints;
        [SerializeField] private Text m_Speed;
        [SerializeField] private Text m_Agility;
        [SerializeField] private Image m_Preview;

        private void Start()
        {
            if (m_Prefab == null) return;

            m_ShipName.text = m_Prefab.Nickname;
            m_Hitpoints.text = "HP : " + m_Prefab.MaxHitPoints.ToString();
            m_Speed.text = "Speed : " + m_Prefab.MaxLinearVelocity.ToString();
            m_Agility.text = "Agility : " + m_Prefab.MaxAngularVelocity.ToString();
            m_Preview.sprite = m_Prefab.PreviewImage;           
        }

        public void SelectShip()
        {
            Player.SelectedSpaceShip = m_Prefab;
            m_MainMenu.ShowMainPanel();
        }

    }
}

   
