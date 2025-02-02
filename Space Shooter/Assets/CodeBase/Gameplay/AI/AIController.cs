using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]

    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Patrol,
            PatrolRoute
        }

        [SerializeField] private AIBehaviour m_AIBehaiour;  //Variable for storing behavior

        [SerializeField] private AIPointPatrol m_PatrolPoint;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear; //Movement speed

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular; //Rotation speed

        [SerializeField] private float m_RandomSelectMovePointTime;

        [SerializeField] private float m_FindNewTargetTime;

        [SerializeField] private float m_ShootDelay;

        [SerializeField] private float m_EvadeRayLength;

        [SerializeField] private Transform[] m_PatrolRoutePoints; 
        private int m_CurrentRoutePointIndex = 0;



        private SpaceShip m_SpaceShip;

        private Vector3 m_MovePosition;

        private Destructible m_SelectedTarget;

        private Timer m_RandomizeDirectionTimer;
        private Timer m_FireTimer;
        private Timer m_FindNewTargetTimer;

        private Vector3 LeadPoint;

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();
            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();
        }

        private void UpdateAI()
        {
            if (m_AIBehaiour == AIBehaviour.Patrol)
            {
                UpdateBehaviourPatrol();
            }

            if (m_AIBehaiour == AIBehaviour.PatrolRoute)
            {
                UpdateBehaviourPatrolRoute();
            }
        }

        private void UpdateBehaviourPatrolRoute()
        {
            ActionControlShip();
            ActionFindNewAttackTarget();
            ActionEvadeCollision();

            if (m_SelectedTarget != null)
            {                              
                
                float distanceToTarget = Vector2.Distance(transform.position, m_SelectedTarget.transform.position);
                                
                if (distanceToTarget < 10.0f) 
                {
                    Vector2 leadPoint = MakeLead(m_SelectedTarget, 2.0f);
                    m_MovePosition = leadPoint;

                    Debug.DrawLine(transform.position, leadPoint, Color.red);

                    //m_MovePosition = m_SelectedTarget.transform.position;
                    ActionFire();

                }
                else 
                {
                    m_SelectedTarget = null;
                    UpdateRoutePoint();
                }
            }
            else 
            {
                UpdateRoutePoint();
            }

            
            
        }

        private void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionControlShip();
            ActionFindNewAttackTarget();
            ActionFire();
            ActionEvadeCollision();
        }

        private void ActionFindNewMovePosition()
        {
            if (m_AIBehaiour == AIBehaviour.Patrol) 
            {
                if (m_SelectedTarget != null)
                {
                    m_MovePosition = m_SelectedTarget.transform.position;
                }
                else
                {
                    if (m_PatrolPoint != null)
                    {
                        bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).sqrMagnitude < m_PatrolPoint.Radius * m_PatrolPoint.Radius;

                        if (isInsidePatrolZone == true)
                        {
                            if (m_RandomizeDirectionTimer.IsFinished == true) 
                            {
                                Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_PatrolPoint.Radius + m_PatrolPoint.transform.position;

                                m_MovePosition = newPoint;

                                m_RandomizeDirectionTimer.StartTimer(m_RandomSelectMovePointTime);
                            }                            
                        }
                        else
                        {
                            m_MovePosition = m_PatrolPoint.transform.position;
                        }
                    }
                }
            
            }
        }

        private void ActionEvadeCollision()
        {
            if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength) == true)
            {
                m_MovePosition = transform.position + transform.right * 100.0f;
            }
        }

        private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;

            

           m_SpaceShip.TorqueControl = ComputeAliginTorqueNormalized(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;
        }

        private const float MAX_ANGLE = 45.0f;
        
        private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);           

            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;            

            return -angle;
        }




        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinished == true)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                m_FindNewTargetTimer.StartTimer(m_FindNewTargetTime);

                //m_FindNewTargetTimer.Start(m_ShootDelay);
            }
        }
        private void ActionFire()
        {
            if (m_SelectedTarget != null)
            {
                if (m_FireTimer.IsFinished == true)
                {
                    m_SpaceShip.Fire(TurretMode.Primary);

                    m_FireTimer.StartTimer(m_ShootDelay);
                }
            }
        }

        private Destructible FindNearestDestructibleTarget()
        {
            float maxDist = float.MaxValue;

            Destructible potentialTarget = null;

            foreach (var v in Destructible.AllDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_SpaceShip) continue;

                if (v.TeamId == Destructible.TeamIdNeutral) continue;

                if (v.TeamId == m_SpaceShip.TeamId) continue;

                float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);

                if (dist < maxDist)
                {
                    maxDist = dist;
                    potentialTarget = v;
                }
                
            }

            return potentialTarget;
        }

        private void UpdateRoutePoint()
        {
            if (m_PatrolRoutePoints.Length == 0) return;
                        
            m_MovePosition = m_PatrolRoutePoints[m_CurrentRoutePointIndex].position;
                        
            if (Vector3.Distance(transform.position, m_MovePosition) < 1.0f)
            {                
                m_CurrentRoutePointIndex = (m_CurrentRoutePointIndex + 1) % m_PatrolRoutePoints.Length;
            }
        }


        private Vector2 MakeLead(Destructible selectedTarget, float TimeLead)
        {
            if (selectedTarget == null) return transform.position; 

            Rigidbody2D targetRigidbody = selectedTarget.GetComponent<Rigidbody2D>();

            if (targetRigidbody == null)
            {
                return selectedTarget.transform.position;
            }
                                    
            Vector2 targetVelocity = targetRigidbody.velocity;
                       
            return (Vector2)selectedTarget.transform.position + targetVelocity * TimeLead;
        }


        #region Timers
        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_FireTimer = new Timer(m_ShootDelay);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
        }

        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            m_FireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
            

        // if (m_AIBehaiour == AIBehaviour.Patrol) UpdateBehaviourPatrol();

    }

        public void SetPatrolBehaviour(AIPointPatrol point)
        {
            m_AIBehaiour = AIBehaviour.Patrol;
            m_PatrolPoint = point;
        }


        #endregion

    }
}


