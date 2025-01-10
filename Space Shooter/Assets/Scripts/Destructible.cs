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
       [SerializeField] private ParticleSystem explosionParticles;

        /// <summary>
        /// The object ignores damage
        /// </summary>
        #region Properties
        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        private bool m_OriginalIndestructibleState;

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
            Destroy(gameObject);
            m_EventOnDeath?.Invoke();
        }

       private void Explode()
        {  
            if (explosionParticles != null)
            {
                ParticleSystem explosionInstance = Instantiate(explosionParticles, transform.position, Quaternion.identity);

                explosionInstance.Play();                
            }
        }

        public void SetIndestructible(bool isIndestructible, float duration = 0f)
        {
            m_OriginalIndestructibleState = m_Indestructible;

            m_Indestructible = isIndestructible;
            
            if (duration > 0f)
            {
                StartCoroutine(ResetIndestructibleAfterDelay(duration));
            }
        }

        
        private IEnumerator ResetIndestructibleAfterDelay(float delay)
        {
            
            yield return new WaitForSeconds(delay);

            
            m_Indestructible = m_OriginalIndestructibleState;

            
            Debug.Log($"m_Indestructible returned to original value: {m_Indestructible}");
        }


        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

    }
}

