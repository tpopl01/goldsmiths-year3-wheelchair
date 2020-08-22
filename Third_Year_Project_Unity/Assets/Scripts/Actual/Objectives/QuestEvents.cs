using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class that handles events that different objectives subscribe to in order to know whether or not they have been completed
namespace tpopl001.Questing
{
    public static class QuestEvents// : MonoBehaviour
    {
        #region Delegate Events
        public delegate void QuestCompleteEvent();
        public static event QuestCompleteEvent OnQuestComplete;
        public static event QuestCompleteEvent OnLoadNewScene;

        public delegate void CollectionAction(string slug);
        public static event CollectionAction OnItemCollected;

        public delegate void LocationUpdate(Vector3 playerPos);
        public static event LocationUpdate OnLocationChange;

        public delegate void TickEvent();
        public static event TickEvent OnTick;

        public delegate void ObjectiveAdvanceEvent();
        public static event ObjectiveAdvanceEvent OnObjectiveAdvance;

        public delegate void LookAtEvent(RaycastHit hit);
        public static event LookAtEvent OnLookAt;

        public delegate void ButtonPressEvent(int id);
        public static event ButtonPressEvent OnButtonPress;

        public delegate void UpdateTurtleEvent(Companion.CompanionState state, Vector3 target, string speech);
        public static event UpdateTurtleEvent OnUpdateTurtle;
        #endregion

        #region Call Events
        /// <summary>
        /// Called whenever an item is collected
        /// </summary>
        public static void ItemCollected(string slug)
        {
            if (OnItemCollected != null)
                OnItemCollected(slug);
        }

        /// <summary>
        /// Called every few frames to inform of the players location
        /// </summary>
        public static void LocationChange(Vector3 playerPos)
        {
            if (OnLocationChange != null)
                OnLocationChange(playerPos);
        }

        /// <summary>
        /// Called every frame. The location objective utilises this to see if the destination has been reached.
        /// </summary>
        public static void Tick()
        {
            if (OnTick != null)
                OnTick();
        }

        /// <summary>
        /// Called once a quest has finished
        /// </summary>
        public static void QuestComplete()
        {
            if (OnQuestComplete != null)
                OnQuestComplete();
        }

        /// <summary>
        /// Called whenever an objective finishes
        /// </summary>
        public static void ObjectiveAdvance()
        {
            OnObjectiveAdvance?.Invoke();
        }

        /// <summary>
        /// Called once the players headset looks at a taget
        /// </summary>
        public static void LookAt(RaycastHit hit)
        {
            OnLookAt?.Invoke(hit);
        }

        /// <summary>
        /// Called the player presses a button or in no hands mode drives near the button.
        /// </summary>
        public static void ButtonPress(int id)
        {
            OnButtonPress?.Invoke(id);
        }

        /// <summary>
        /// Called by the objectives to notify the companion that it needs to move and display a tooltip
        /// </summary>
        public static void UpdateTurtle(Companion.CompanionState state, Vector3 target, string speech = "")
        {
            OnUpdateTurtle?.Invoke(state, target, speech);
        }

        public static void LoadNewScene()
        {
            if (OnLoadNewScene != null)
                OnLoadNewScene();
        }
        #endregion
    }
}