using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;

    [SerializeField] private Transform m_Target;

    [SerializeField] private float m_InterpolationLinear;    
    
    [SerializeField] private float m_InteroplationAngular;

    [SerializeField] private float m_CameraZOffset;
    
    [SerializeField] private float m_ForwardOffset;

    private void FixedUpdate()
    {
        if (m_Target == null || m_Camera == null) return;

        Vector2 camPos = m_Camera.transform.position;
        Vector2 targetPos = m_Target.position + m_Target.transform.up * m_ForwardOffset;

        Vector2 newCamPos = Vector2.Lerp(camPos, targetPos, m_InterpolationLinear * Time.deltaTime);

        m_Camera.transform.position = new Vector3(newCamPos.x, newCamPos.y, m_CameraZOffset);

        if (m_InteroplationAngular> 0)
        {
            m_Camera.transform.rotation = Quaternion.Slerp(m_Camera.transform.rotation, m_Target.rotation, m_InteroplationAngular * Time.deltaTime);
        }

    }

    public void SetTarget(Transform newTarget)
    {
        m_Target = newTarget;
    }

}
