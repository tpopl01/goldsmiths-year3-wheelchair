using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Questing
{
    [CreateAssetMenu(menuName = "Quests/Objectives/Angle")]
    public class AngleObjective : Objective
    {
        [Space]
        [Header("The maximum angle the user has to be to be facing the target")]
        [SerializeField][Range(0.1f, 30f)] private float maxAngle = 0.1f;
        private float angle = 0;
        PlayerSettings playerSettings;

        #region clean up
        protected override void OnLoadNewScene()
        {
            base.OnLoadNewScene();
            QuestEvents.OnTick -= Tick;
        }

        void OnDestroy()
        {
            QuestEvents.OnTick -= Tick;
        }
        #endregion

        private void Tick()
        {
            if (CheckCompleted())
            {
                UpdateCompleted();
            }
            //calls the event, this will make it so the visual UI degrees you are compared to the turtle will update
            QuestEvents.ObjectiveAdvance();
        }

        public override bool CheckCompleted()
        {
            //get direction vector
            Vector3 directionToLookTo = GetObjectivePosition() - playerSettings.Wheelchair.transform.position;
            directionToLookTo.y = 0;

            //calculate the angle between the two direction vectors
            angle = Vector3.Angle(playerSettings.Wheelchair.transform.forward, directionToLookTo);
            if (angle > maxAngle)
            {
                return false;
            }
            return true;
        }

        public override string GetDescription()
        {
            return base.GetDescription() + " Angle: " + Mathf.FloorToInt(angle) + " degrees";
        }

        public override void SetUpObjective()
        {
            base.SetUpObjective();
            if(!playerSettings)
                playerSettings = Resources.Load<PlayerSettings>("Settings/PlayerSettings");
            EnableQuestMarkers();
            QuestEvents.OnTick += Tick;
        }

        protected override void UpdateCompleted()
        {
            if (UpdatedCompletion) return;
            QuestEvents.OnTick -= Tick;
            base.UpdateCompleted();
        }

        public override void EnableQuestMarkers()
        {
            if (GetObjectivePosition() != Vector3.zero)
            {
                SpawnQuestMarker(GetObjectivePosition());
            }
        }
    }
}