using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public abstract class ProjectileBase : Entity
    {
        [SerializeField] private float m_Velocity;
        [SerializeField] private float m_Lifetime;
        [SerializeField] private int m_Damage;        
     
        protected virtual void OnHit(Destructible destructible) { }
        protected virtual void OnHit(Collider2D collider2D) { }
        protected virtual void OnProjectileLifeEnd(Collider2D col, Vector2 pos) { }

        [Header("Homing Settings")]
        [SerializeField] private bool isHoming = false;
        [SerializeField] private float m_TurnSpeed = 5f; 

        private float m_Timer;
        private Transform m_Target;
        protected Destructible m_Parent;

        public void SetTarget(Transform target)
        {
            m_Target = target; 
        }

        private void Update()
        {
            if (isHoming && m_Target != null)
            {                
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
                OnHit(hit.collider);

                Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();
                if (dest != null && dest != m_Parent)
                {
                    dest.ApplyDamage(m_Damage);

                    OnHit(dest);
                }

                OnProjectileLifeEnd(hit.collider, hit.point);
            }

            m_Timer += Time.deltaTime;

            if (m_Timer > m_Lifetime)
            {
                OnProjectileLifeEnd(hit.collider, hit.point);
            }

            transform.position += new Vector3(step.x, step.y, 0);
        } 

        public void SetParentShooter(Destructible parent) 
        {
            m_Parent = parent;        
        }
    }
}


