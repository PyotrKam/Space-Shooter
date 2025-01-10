using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Turret : Projectile
    {
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private TurretProperties m_TurretProperties;

        private float m_RefireTimer;
        public bool CanFire => m_RefireTimer <= 0;

        private SpaceShip m_Ship;
        
        private bool m_SecondaryTurret;
        private float m_TurnSpeedRocket;

        #region Unity Event
        private void Start()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>();
        }

        private void Update()
        {
            if (m_SecondaryTurret)
            {
                Transform closestTarget = FindClosestTarget();
                if (closestTarget != null)
                {
                    Vector2 direction = (Vector2)closestTarget.position - (Vector2)transform.position;
                    direction.Normalize();
                    transform.up = Vector3.Lerp(transform.up, direction, Time.deltaTime * m_TurnSpeedRocket);
                }
            }

            if (m_RefireTimer > 0)
                m_RefireTimer -= Time.deltaTime;
        }
        #endregion

        // Public API
        public void Fire()
        {
            if (m_TurretProperties == null) return;
            
            if (m_RefireTimer > 0) return;

            if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false)
            {
                return;
            }

            if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false)
            {
                return;
            }

            Transform closestTarget = FindClosestTarget();


            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            if (closestTarget != null)
            {
                projectile.SetTarget(closestTarget);
            }

            projectile.SetParentShooter(m_Ship);

            m_RefireTimer = m_TurretProperties.RateOfFire;

            {
                // SFX
            }
        }

        public void AssignLoadout(TurretProperties props)
        {
            if (m_Mode != props.Mode) return;

            m_RefireTimer = 0;
            m_TurretProperties = props;

            Debug.Log("Turret " + name + " assigned loadout: " + props.name);
        }
        
        private Transform FindClosestTarget()
        {
            Transform closestTarget = null;
            float closestDistance = Mathf.Infinity;

            GameObject[] targets = GameObject.FindGameObjectsWithTag("Asteroids");

            //Debug.Log($"Find target: {targets.Length}");

            foreach (GameObject target in targets)
            {
                if (target == null) continue;

                float distance = Vector3.Distance(transform.position, target.transform.position);


                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target.transform;
                }
            }

            return closestTarget;
        }
        
    }
}
