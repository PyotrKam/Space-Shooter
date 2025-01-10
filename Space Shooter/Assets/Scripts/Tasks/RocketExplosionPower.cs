using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpaceShooter
{
    public class RocketExplosionPower : MonoBehaviour
    {
        [SerializeField] private float radius = 5f;
        [SerializeField] private float maxDamage = 100f;
        [SerializeField] private float explosionForce = 700f;
        [SerializeField] private GameObject explosionEffect;

        private Rigidbody2D rb;


        [SerializeField] private float colliderEnableDelay = 0.1f; 
        private Collider2D rocketCollider;


        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D cant find on Rocket!");
            }
            else
            {
                Debug.Log("Rigidbody2D found!");
            }

            rocketCollider = GetComponent<Collider2D>();
            rocketCollider.enabled = false; 
            Invoke("EnableCollider", colliderEnableDelay); 
        }

        private void EnableCollider()
        {
            rocketCollider.enabled = true;
            Debug.Log("Rocket collider enabled!");
        }

        private void Explode()
        {
            Debug.Log("Explosion triggered!");

            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

            foreach (Collider2D nearbyObject in colliders)
            {
                Debug.Log("Find new object in radius " + nearbyObject.name);

                if (nearbyObject.CompareTag("SpaceShip")) continue;

                float distance = Vector2.Distance(nearbyObject.transform.position, transform.position);
                float damage = maxDamage * (1 - distance / radius);
                if (damage > 0)
                {
                    Destructible destructible = nearbyObject.GetComponent<Destructible>();
                    if (destructible != null)
                    {
                        Debug.Log("Applying damage to: " + nearbyObject.name);
                        destructible.ApplyDamage((int)damage);
                    }
                }

                Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Debug.Log("Applying force to: " + nearbyObject.name);
                    Vector2 direction = (nearbyObject.transform.position - transform.position).normalized;
                    rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
                }
            }

            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Collision detected with: " + collision.gameObject.name);

            
            if (collision.gameObject.CompareTag("Asteroids"))
            {
                Debug.Log("Collided with an asteroid!");
            }
            else
            {
                Debug.Log("Collided with something else: " + collision.gameObject.tag);
            }
                       
            Explode();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

}



