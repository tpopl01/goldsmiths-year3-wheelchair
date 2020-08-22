using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Questing
{
    [CreateAssetMenu(menuName = "Quests/Objectives/Button Controller")]
    public class ButtonPressObjective : Objective
    {
        [SerializeField] private OVRInput.Button button = OVRInput.Button.Any;
        private bool complete;

        protected override void OnLoadNewScene()
        {
            base.OnLoadNewScene();
            QuestEvents.OnTick -= Tick;
        }
        private void OnDestroy()
        {
            QuestEvents.OnTick -= Tick;
        }

        //wait until the button is pressed then complete the objective
        protected virtual void Tick()
        {
            if(OVRInput.Get(button))
            {
                complete = true;
                UpdateCompleted();
            }
        }

        public override void SetUpObjective()
        {
            if(StaticLevel.noHands)
            {
                complete = true;
                UpdateCompleted();
                return;
            }

            base.SetUpObjective();
            QuestEvents.OnTick += Tick;
            complete = false;
        }

        protected override void UpdateCompleted()
        {
            QuestEvents.OnTick -= Tick;
            base.UpdateCompleted();
        }

        public override bool CheckCompleted()
        {
            return complete;
        }

        //prevent any quest markers from spawning
        public override void EnableQuestMarkers()
        {
            
        }
    }
}