using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class EntitySpawnerDebris : MonoBehaviour
    {
        [SerializeField] private Destructible[] m_DebrisPrefabs;

        [SerializeField] private CircleArea m_Area;

        [SerializeField] private int m_NumDebris;       

        [SerializeField] private float m_RandomSpeed;

        private void Start()
        {
            for (int i = 0; i < m_NumDebris; i++)
            {
                SpawnDebirs();
            }
        }

        private void SpawnDebirs()
        {
            int index = Random.Range(0, m_DebrisPrefabs.Length);

            GameObject debris = Instantiate(m_DebrisPrefabs[index].gameObject);

            debris.transform.position = m_Area.GetRandomInsideZone();
            debris.GetComponent<Destructible>().EventOnDeath.AddListener(OnDebrisDead);

            Rigidbody2D rb = debris.GetComponent<Rigidbody2D>();

            if (rb != null && m_RandomSpeed >0)
            {
                rb.velocity = (Vector2)UnityEngine.Random.insideUnitSphere * m_RandomSpeed;
            }
        }

        private void OnDebrisDead()
        {
            SpawnDebirs();
        }
    }

}

