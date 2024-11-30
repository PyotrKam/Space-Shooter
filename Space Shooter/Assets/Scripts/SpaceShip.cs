using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        /// <summary>
        /// Mass for automatic installation at the rigid
        /// </summary>
        [Header("Space ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// Forward pushing force
        /// </summary>
        [SerializeField] private float m_Thrust;

        /// <summary>
        /// Rotational force
        /// </summary>
        [SerializeField] private float m_Mobility;

        /// <summary>
        /// Maximum linear speed
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;

        /// <summary>
        /// Maximum rotational speed. In degrees/sec
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;

        /// <summary>
        /// Saved link to the rigid.
        /// </summary>
        private Rigidbody2D m_Rigid;

        #region Public API

        /// <summary>
        /// Linear thrust control. -1.0 to +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Rotary thrust control. -1.0 to +1.0
        /// </summary>
        public float TorqueControl { get; set; }

        #endregion

        #region Unity Event
        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();  //Component caching
            m_Rigid.mass = m_Mass; //Set the mass

            m_Rigid.inertia = 1; //Set the inerial force, to make it easier to balance the balance of power

        }

        private void Update()
        {
            ThrustControl = 0;
            TorqueControl = 0;


            if (Input.GetKey(KeyCode.UpArrow)) ThrustControl = 1.0f;

            if (Input.GetKey(KeyCode.DownArrow)) ThrustControl = -1.0f;

            if (Input.GetKey(KeyCode.LeftArrow)) TorqueControl = 1.0f;

            if (Input.GetKey(KeyCode.RightArrow)) TorqueControl = -1.0f;


        }


        private void FixedUpdate() //Update not in frames, but in a certain amount of time
        {
            UpdateRigidBody();
        }

        #endregion

        /// <summary>
        /// Method of adding forces to a ship for movement
        /// </summary>
        private void UpdateRigidBody()   //To control spaceship
        {
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force); //Pushing force

            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force); //Forces for the opposite direction (resistance)

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force); //Rotation of the ship

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force); //Rotation of the ship to another side

        }


    }

}
