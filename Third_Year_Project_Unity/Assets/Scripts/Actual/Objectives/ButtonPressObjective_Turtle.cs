using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Questing
{
    [CreateAssetMenu(menuName = "Quests/Objectives/Button Controller Turtle")]
    public class ButtonPressObjective_Turtle : ButtonPressObjective
    {
        [Space]
        [Header("Companion")]
        [SerializeField] private string turtle_speech = "";

        void OnDestroy()
        {
            QuestEvents.OnTick -= Tick;
        }

        protected override void Tick()
        {
            base.Tick();
            //updates the companions speech bubble
            QuestEvents.UpdateTurtle(Companion.CompanionState.LookAtPlayer, GetObjectivePosition(), turtle_speech);
        }

        protected override void UpdateCompleted()
        {
            base.UpdateCompleted();
            QuestEvents.UpdateTurtle(Companion.CompanionState.LookAtPlayer, GetObjectivePosition(), "");
        }

    }
}