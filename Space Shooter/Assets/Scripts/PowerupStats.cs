using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PowerupStats : Powerup
    {
        public enum EffectType 
        { 
            AddAmmo,
            AddEnergy,
            Immortality
        }

        [SerializeField] private EffectType m_EffectType;

        [SerializeField] private float m_Value;

        [SerializeField]  public Destructible destructibleObject;

        protected override void OnPickedUp(SpaceShip ship)
        {
            if (!ship.gameObject.activeInHierarchy)
            {
                ship.gameObject.SetActive(true);
            }

            Destructible destructibleObject = ship.GetComponent<Destructible>();


            if (m_EffectType == EffectType.AddEnergy)
            {
                ship.AddEnergy((int) m_Value);
            }

            if (m_EffectType == EffectType.AddAmmo)
            {
                ship.AddAmmo((int)m_Value);
            }

            if (m_EffectType == EffectType.Immortality)
            {
                destructibleObject.SetIndestructible(true, 10f);
            }
        }
    }

}


