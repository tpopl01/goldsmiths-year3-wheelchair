using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Questing
{
    [CreateAssetMenu(menuName = "Quests/Objectives/Location")]
    public class LocationObjective : Objective
    {
        protected float distanceToTarget = 100;

        [Space]
        [Header("Distance")]
        [Tooltip("Max distance from destination for the objective to be marked as complete")]
        [SerializeField]
        private float maxDistanceFromTargetLocation = 5;

        #region CleanUp
        //if the user quits the game before the objective is complete
        //Remove the subscription to the event
        void OnDestroy()
        {
            QuestEvents.OnLocationChange -= LocationObject;
        }
        protected override void OnLoadNewScene()
        {
            base.OnLoadNewScene();
            QuestEvents.OnLocationChange -= LocationObject;
        }
        #endregion

        #region Protected
        /// <summary>
        /// Checks the distance of the player to the destination
        /// Updates completion accordingly
        /// </summary>
        /// <param name="radius">The radius of the wheel</param>
        protected virtual void LocationObject(Vector3 playerPos)
        {
            distanceToTarget = Vector3.Distance(playerPos, GetObjectivePosition());
            if (CheckCompleted())
            {
                UpdateCompleted();
            }
            QuestEvents.ObjectiveAdvance();
        }

        protected override void UpdateCompleted()
        {
            if (UpdatedCompletion) return;
            QuestEvents.OnLocationChange -= LocationObject;
            base.UpdateCompleted();
        }
        #endregion

        #region Public Overrides
        public override bool CheckCompleted()
        {
            return distanceToTarget < maxDistanceFromTargetLocation;
        }

        public override string GetDescription()
        {
            return base.GetDescription() + " The current distance to the target is: " + Mathf.FloorToInt(distanceToTarget);
        }

        public override void SetUpObjective()
        {
            base.SetUpObjective();
            EnableQuestMarkers();
            distanceToTarget = 100;
            QuestEvents.OnLocationChange += LocationObject;
        }


        public override void EnableQuestMarkers()
        {
            if (GetObjectivePosition() != Vector3.zero)
            {
                SpawnQuestMarker(GetObjectivePosition());
            }
        }
        #endregion
    }
}