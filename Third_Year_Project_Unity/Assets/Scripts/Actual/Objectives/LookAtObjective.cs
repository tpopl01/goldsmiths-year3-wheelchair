using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Questing
{
    [CreateAssetMenu(menuName = "Quests/Objectives/LookAt")]
    public class LookAtObjective : Objective
    {
        [SerializeField][Tooltip("The name of the object to look at")] private string lookAtObj = "";
        private bool complete = false;

        private void OnDestroy()
        {
            QuestEvents.OnLookAt -= LookAt;
        }
        protected override void OnLoadNewScene()
        {
            base.OnLoadNewScene();
            QuestEvents.OnLookAt -= LookAt;
        }

        //called from an event, is the object the raycast hit our target object
        private void LookAt(RaycastHit hit)
        {
            if (complete) return;

            if(hit.transform.name.Equals(lookAtObj))
            {
                complete = true;
                UpdateCompleted();
            }
        }

        public override bool CheckCompleted()
        {
            if (complete)
            {
                UpdateCompleted();
                return true;
            }
            return false;
        }

        public override void SetUpObjective()
        {
            base.SetUpObjective();
            QuestEvents.OnLookAt += LookAt;
            complete = false;
            EnableQuestMarkers();
        }

        protected override void UpdateCompleted()
        {
            if (!UpdatedCompletion)
            {
                QuestEvents.OnLookAt -= LookAt;
            }
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