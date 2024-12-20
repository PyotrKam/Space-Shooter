using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpaceShooter
{
    public class MovementCntroller : MonoBehaviour
    {
        public enum ControlMode 
        {
            Keyboard,
            Mobile       
        }



        [SerializeField] private SpaceShip m_TargetShip;
        public void SetTargetShip(SpaceShip ship) => m_TargetShip = ship;

        [SerializeField] private VirtualJoystick m_MobileJoystick;

        [SerializeField] private ControlMode m_ControlMode;


        private void Start()
        {
            if (m_ControlMode == ControlMode.Keyboard)
            {
                m_MobileJoystick.gameObject.SetActive(false);
            }
            else
            {
                m_MobileJoystick.gameObject.SetActive(true);
            }

        }


        private void Update()
        {
            if (m_TargetShip == null) return;

             if (m_ControlMode == ControlMode.Keyboard) ControlKeyboard();

             if (m_ControlMode == ControlMode.Mobile) ControlMobile();
        }

        private void ControlMobile()
        {
            Vector3 dir = m_MobileJoystick.Value;

            var dot = Vector2.Dot(dir, m_TargetShip.transform.up);
            var dot2 = Vector2.Dot(dir, m_TargetShip.transform.right);

            m_TargetShip.ThrustControl = Mathf.Max(0, dot);
            m_TargetShip.TorqueControl = -dot2;

        }

        private void ControlKeyboard()
        {
            float thrust = 0;
            float torque = 0;

            if (Input.GetKey(KeyCode.UpArrow)) thrust = 1.0f;

            if (Input.GetKey(KeyCode.DownArrow)) thrust = -1.0f;

            if (Input.GetKey(KeyCode.LeftArrow)) torque = 1.0f;

            if (Input.GetKey(KeyCode.RightArrow)) torque = -1.0f;

            m_TargetShip.ThrustControl = thrust;
            m_TargetShip.TorqueControl = torque;

        }


    }

}
