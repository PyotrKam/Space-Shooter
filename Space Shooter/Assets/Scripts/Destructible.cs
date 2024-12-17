using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    /// <summary>
    /// An object that can be destroyed. Something that has hitpoints.
    /// </summary>
    public class Destructible : Entity
    {
       [SerializeField] private GameObject explosionParticles;


        /// <summary>
        /// The object ignores damage
        /// </summary>
        #region Properties
        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Starting hitpoints
        /// </summary>
        [SerializeField] private int m_HitPoints;

        /// <summary>
        /// Current hitpoints
        /// </summary>
        private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;

        
        #endregion

        #region Unity Events
        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;
        }
        #endregion

        #region Public API
        /// <summary>
        /// Applying damage to an object
        /// </summary>
        /// <param name="damage"> Damage to an oject</param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
            {
                Explode();

                OnDeath();
            }

        }
        #endregion
        /// <summary>
        /// The event defined when the points are zero or below zero
        /// </summary>
        protected virtual void OnDeath()
        {
            Debug.Log(" Death");
            Destroy(gameObject);
            m_EventOnDeath?.Invoke();
        }

       private void Explode()
        {
            

            if (explosionParticles != null)
            {
                
                GameObject explotion = Instantiate(explosionParticles, transform.position, Quaternion.identity);
                Debug.Log("Create an instance of the Particle System prefab at the object position");

                ParticleSystem particleSystem = explosionParticles.GetComponent<ParticleSystem>();
                Debug.Log("We get the Particle System component");


                particleSystem.Play();
                Debug.Log("Producing the Particle System");




                //Debug.Log("Explode");
            }


        }
       


            [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

    }
}

