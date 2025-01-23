using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace Common
{
    public class PointerClickHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool m_Hold;
        public bool IsHold => m_Hold;

        public void OnPointerDown(PointerEventData eventData)
        {
            m_Hold = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_Hold = false;
        }
    }
}

   
