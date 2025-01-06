using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpaceShooter
{
    public class Projectile : Entity
    {
        [SerializeField] private float m_Velocity;

        [SerializeField] private float m_Lifetime;

        [SerializeField] private int m_Damage;

        [SerializeField] private ImpactEffect m_ImpactEffectPrefab;

        [Header("Homing Settings")]
        [SerializeField] private bool isHoming = false;
        [SerializeField] private float m_TurnSpeed = 5f; 


        private float m_Timer;

        private Transform m_Target;

        public void SetTarget(Transform target)
        {
            m_Target = target; 
        }


        private void Update()
        {

            if (isHoming && m_Target != null)
            {
                Debug.Log($"The missile is guided to the target: {m_Target.name}");
                Vector2 direction = (Vector2)m_Target.position - (Vector2)transform.position;

                Rigidbody2D targetRigidbody = m_Target.GetComponent<Rigidbody2D>();
                if (targetRigidbody != null)
                {
                    direction += (Vector2)targetRigidbody.velocity * Time.deltaTime;
                }

                direction.Normalize();

                
                transform.up = Vector3.Lerp(transform.up, direction, Time.deltaTime * m_TurnSpeed);
            }





            float stepLength = Time.deltaTime * m_Velocity;

            Vector2 step = transform.up * stepLength;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength); //start point, direction and length

            if (hit)
            {
                Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();
                if (dest != null && dest != m_Parent)
                {
                    dest.ApplyDamage(m_Damage);
                }

                OnProjectileLifeEnd(hit.collider, hit.point);

            }

            m_Timer += Time.deltaTime;

            if (m_Timer > m_Lifetime)
            {
                Destroy(gameObject);
            }

            transform.position += new Vector3(step.x, step.y, 0);
        }

        protected void OnProjectileLifeEnd(Collider2D col, Vector2 pos) 
        {
            Destroy(gameObject);
        
        }

        private Destructible m_Parent;

        public void SetParentShooter(Destructible parent) 
        {
            m_Parent = parent;
        
        }
    }

}


