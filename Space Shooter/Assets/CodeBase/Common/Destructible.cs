using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Common
{
    /// <summary>
    /// An object that can be destroyed. Something that has hitpoints.
    /// </summary>
    public class Destructible : Entity
    {
        [SerializeField] private ParticleSystem explosionParticles;

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;


        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;

        /// <summary>
        /// The object ignores damage
        /// </summary>
        #region Properties
        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        private bool m_IsTimerRunning = false;
        private float m_TimerDuration;
        private float m_TimerEndTime;
        private bool _isDead;

        /// <summary>
        /// Starting hitpoints
        /// </summary>
        [SerializeField] private int m_HitPoints;
        public int MaxHitPoints => m_HitPoints;
        
        /// <summary>
        /// Current hitpoints
        /// </summary>
        private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;

        private void Update()
        {
            if (m_IsTimerRunning)
            {
                if (Time.time >= m_TimerEndTime)
                {
                    OnTimerEnd();
                    m_IsTimerRunning = false;
                }
            }
        }

        #endregion

        #region Unity Events
        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;

            transform.SetParent(null);
        }
        #endregion

        #region Public API
        /// <summary>
        /// Applying damage to an object
        /// </summary>
        /// <param name="damage"> Damage to an oject</param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible || _isDead) 
                return;

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
            {
                Explode();
                OnDeath();
                _isDead = true;
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

        private static HashSet<Destructible> m_AllDestructibles;

        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
            {
                m_AllDestructibles = new HashSet<Destructible>();
            }

            m_AllDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

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
            m_Indestructible = isIndestructible;

            if (duration > 0f)
            {
                StartTimer(duration);
            }
        }

        private void StartTimer(float duration)
        {
            m_TimerDuration = duration;
            m_TimerEndTime = Time.time + duration;
            m_IsTimerRunning = true;
        }

        private void OnTimerEnd()
        {
            Debug.Log("Timer ended!");
            m_Indestructible = false;
        }




        

    }
}

