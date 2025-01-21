using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelCompletionPosition : LevelCondition
    {
        [SerializeField] private float m_Radius;

        public override bool IsCompleted
        { 
            get
            {
                if (Player.Instance.ActiveShip == null) return false;

                if (Vector3.Distance(Player.Instance.ActiveShip.transform.position, transform.position) <= m_Radius)
                {
                    return true;
                }

                return false;
            }
        
        }

#if UNITY_EDITOR
        private static Color GizmoColor = new Color(0, 1, 0, 0.3f);

        private void OnDrawGizmosSelected()
        {
            Handles.color = GizmoColor;
            Handles.DrawSolidDisc(transform.position, transform.forward, m_Radius);
        }
#endif


    }
}


