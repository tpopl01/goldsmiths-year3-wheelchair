using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using tpopl001.Questing;
using UnityEngine.UI;

namespace tpopl001.Companion
{
    //State based controller for a companion
    public class Companion : MonoBehaviour
    {
        #region Variables

        NavMeshAgent agent;

        CompanionState state = CompanionState.LookAtPlayer;

        Vector3 targetPos = Vector3.zero;

        [SerializeField] Text speech_text = null;
        [SerializeField] GameObject speech_canvas = null;
        PlayerSettings playerSettings;

        #endregion

        #region Built In Methods

        /// <summary>
        /// Method removes any references when the object is destroyed.	
        /// Unsubscribes the event delegate
        /// </summary>
        void OnDestroy()
        {
            QuestEvents.OnUpdateTurtle -= UpdateTurtle;
        }

        /// <summary>
        /// Initialise variables and subscribe companion update to an event.
        /// The event is called by the objectives
        /// </summary>
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            QuestEvents.OnUpdateTurtle += UpdateTurtle;

            speech_canvas.SetActive(false);
            playerSettings = Resources.Load<PlayerSettings>("Settings/PlayerSettings");
        }

        /// <summary>
        /// Call the state logic each frame	
        /// </summary>
        void Update()
        {
            StateManager();
        }
        #endregion

        #region State Machine
        /// <summary>
        /// Simple state machine for handling the companions states   	
        /// </summary>
        void StateManager()
        {
            switch (state)
            {
                case CompanionState.LookAtPlayer:
                    agent.updateRotation = false;
                    LookAtTarget(playerSettings.HMD.position);
                    break;
                case CompanionState.MoveToPosition:
                    if (agent.remainingDistance < 0.5f)
                    {
                        state = CompanionState.LookAtPlayer;
                    }
                    break;
            }
        }
        #endregion

        #region Utility
        /// <summary>
        /// Activates the companions speech bubble.   	
        /// </summary>
        /// <param name="speech">What the companion should say</param>
        void AcivateSpeech(string speech)
        {
            speech_canvas.SetActive(true);
            speech_text.text = speech;
        }
        #endregion

        #region Navigation
        /// <summary>
        /// Moves the companion to the target position.  	
        /// </summary>
        /// <param name="pos">Target position to move to</param>
        void MoveToPosition(Vector3 pos)
        {
            NavMeshHit hit;
            // sample a valid pathfinding spot around a given location
            if (NavMesh.SamplePosition(pos, out hit, 5, NavMesh.AllAreas))
            {
                agent.updateRotation = true;
                state = CompanionState.MoveToPosition;
                agent.SetDestination(hit.position);
            }
            else
            {
                Debug.LogWarning("Unable to move turtle to location " + pos);
            }
        }

        /// <summary>
        /// Smoothly interpolates the companion rotation to face the target position   	
        /// </summary>
        /// <param name="target">The position to look at</param>
        void LookAtTarget(Vector3 target)
        {
            Quaternion lookAngle = StaticCalculations.GetLookRotation(target, transform.position, transform.forward, out bool rotate);
            if (rotate)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, lookAngle, Time.deltaTime * 5);
            }
        }
        #endregion

        #region Event
        /// <summary>
        /// Updates the turtles state, position and creates a speech bubble if any text is passed.   	
        /// </summary>
        /// <param name="state">Desired companion state</param>
        /// <param name="targetPos">Desired companion position</param>
        /// <param name="speech">What the companion should say</param>
        void UpdateTurtle(CompanionState state, Vector3 targetPos, string speech)
        {
            if (!string.IsNullOrEmpty(speech))
            {
                if (!speech.Equals(speech_text.text))
                {
                    AcivateSpeech(speech);
                }
            }
            else
            {
                speech_canvas.SetActive(false);
            }

            switch (state)
            {
                case CompanionState.LookAtPlayer:
                    break;
                case CompanionState.MoveToPosition:
                    if (this.targetPos != targetPos)
                    {
                        this.targetPos = targetPos;
                        MoveToPosition(targetPos);
                    }
                    break;
            }
        }
        #endregion
    }

    public enum CompanionState
    {
        LookAtPlayer,
        MoveToPosition
    }
}