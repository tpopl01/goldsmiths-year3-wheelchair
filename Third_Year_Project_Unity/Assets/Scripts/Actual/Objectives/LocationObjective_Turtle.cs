using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Questing
{
    [CreateAssetMenu(menuName = "Quests/Objectives/Location Turtle")]
    public class LocationObjective_Turtle : LocationObjective
    {
        [SerializeField] private string turtle_speech = "";
        private void OnDestroy()
        {
            QuestEvents.OnLocationChange -= LocationObject;
        }
        protected override void LocationObject(Vector3 playerPos)
        {
            if (UpdatedCompletion) return;
            //set up turtles speech
            QuestEvents.UpdateTurtle(Companion.CompanionState.MoveToPosition, GetObjectivePosition(), turtle_speech);
            base.LocationObject(playerPos + Vector3.forward);
        }

        protected override void UpdateCompleted()
        {
            base.UpdateCompleted();
            QuestEvents.UpdateTurtle(Companion.CompanionState.MoveToPosition, GetObjectivePosition(), "");
        }
    }
}