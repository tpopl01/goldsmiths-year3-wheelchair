using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace tpopl001.Companion {
    //makes Unity automatically add the NavMeshAgent component to the object this script is put on
    [RequireComponent(typeof(NavMeshAgent))] 
    //This class will send a unit to different waypoints. The unit will wait there for a random amount of time.
    public class WaypointObstacle : MonoBehaviour
    {
        #region Variables
        private float waitTime = 0;
        private Vector3 targetPosition;
        private NavMeshAgent agent;
        private bool isWaiting;
        private Animator anim;


        [Space]
        [Header("Waiting")]
        [SerializeField] [Range(2, 12)] private float waitTimeMin = 10;
        [SerializeField] [Range(12, 20)] private float waitTimeMax = 12;
        [SerializeField] [Range(0, 100)] private int bypassWaitChance = 25;

        [Space]
        [Header("NavMesh Agent Speed")]
        [SerializeField] private float walkSpeed = 0.3f;
        [SerializeField] private float trottSpeed = 1.5f;
        [SerializeField] private float gallopSpeed = 3f;

        [Space]
        [Header("Other")]
        [SerializeField] private Transform[] waypoints = null;
        #endregion

        #region Initialise
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponentInChildren<Animator>();
            ChangeWaypoint();
        }
        #endregion

        #region Handle Waypoints
        void Update()
        {
            if (agent.isOnNavMesh)
                if (agent.remainingDistance < 1)
                {
                    //wait then change waypoint
                    if(!isWaiting)
                    {
                        TryWait();
                    }
                    else
                    {
                        AtWaypoint();
                    }
                }
        }

        /// <summary>
        /// Attempt to wait at the waypoint and adjust the agents parameters accordingly
        /// </summary>
        private void TryWait()
        {
            waitTime = DetermineWaitTime();
            if (waitTime > 0)
            {
                anim.SetFloat("vertical", 0);
                isWaiting = true;
            }
            else
                ChangeWaypoint();
        }

        /// <summary>
        /// Timer for waiting at a waypoint
        /// </summary>
        private void AtWaypoint()
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                isWaiting = false;
                waitTime = 0;
                ChangeWaypoint();
            }
        }

        /// <summary>
        /// Random chance for the agent to wait for a random amount of time
        /// </summary>
        private float DetermineWaitTime()
        {
            int r = Random.Range(0, 100);
            if (r < bypassWaitChance)
            {
                return 0;
            }
            agent.isStopped = true;
            return Random.Range(waitTimeMin, waitTimeMax);
        }

        /// <summary>
        /// The agent will move to a random waypoint, with a random animation speed.
        /// </summary>
        private void ChangeWaypoint()
        {
            if (waypoints.Length > 0)
            {
                targetPosition = waypoints[Random.Range(0, waypoints.Length)].position;
                if(agent.isOnNavMesh)
                agent.SetDestination(targetPosition);
                int rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    anim.SetFloat("vertical", 0.3f);
                    agent.speed=walkSpeed;
                }
                else if (rand == 1)
                {
                    anim.SetFloat("vertical", 0.5f);
                    agent.speed = trottSpeed;
                }
                else
                {
                    anim.SetFloat("vertical", 1f);
                    agent.speed = gallopSpeed;
                }
                if (agent.isOnNavMesh)
                    agent.isStopped = false;
            }
        }
        #endregion
    }
}