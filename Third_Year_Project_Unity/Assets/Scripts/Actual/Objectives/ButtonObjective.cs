using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Questing
{
    [CreateAssetMenu(menuName = "Quests/Objectives/Button")]
    [System.Serializable]
    public class ButtonObjective : Objective
    {
        [Space]
        [Header("ID")]
        [Tooltip("Must match the id of the desired button to press")][SerializeField] private int button_id = 0;
        private bool complete = false;

        #region Getter
        public int GetButtonID()
        {
            return button_id;
        }
        #endregion

        #region Clean Up
        protected override void OnLoadNewScene()
        {
            base.OnLoadNewScene();
            QuestEvents.OnButtonPress -= ButtonPressed;
        }
        void OnDestroy()
        {
            QuestEvents.OnButtonPress -= ButtonPressed;
        }
        #endregion

        void ButtonPressed(int id)
        {
            //Called from an event
            //if the pressed buttons id matches this id then complete
            if(id == button_id)
            {
                complete = true;
                UpdateCompleted();
            }
        }

        public override bool CheckCompleted()
        {
            return complete;
        }

        public override void SetUpObjective()
        {
            base.SetUpObjective();
            EnableQuestMarkers();
            complete = false;
            QuestEvents.OnButtonPress += ButtonPressed;
        }

        protected override void UpdateCompleted()
        {
            QuestEvents.OnButtonPress -= ButtonPressed;
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