using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Audio;


namespace tpopl001.Questing
{
    //base class for objectives
    public abstract class Objective : ScriptableObject
    {
        #region Variables
        [Header("General")]
        [SerializeField] private string title = "";
        [SerializeField] [TextArea] private string description = "";

        [Space]
        [Header("Setup")]
        [Tooltip("Objective Position")] [SerializeField] private Vector3 pos = Vector3.zero;
        [SerializeField] private string questMarker_slug = "quest_marker_1";

        private List<GameObject> questMarkers = new List<GameObject>();

        protected bool UpdatedCompletion { get; private set; } = false;
        #endregion

        #region Getters
        protected Vector3 GetObjectivePosition()
        {
            return pos;
        }

        public string GetTitle()
        {
            return title;
        }
        public virtual string GetDescription()
        {
            return description;
        }
        #endregion

        #region Public Methods   
        /// <summary>
        /// Destroy all quest markers in the scene
        /// </summary>
        public void DisableQuestMarkers()
        {
            for (int i = 0; i < questMarkers.Count; i++)
            {
                Destroy(questMarkers[i].gameObject);
            }
        }

        /// <summary>
        /// Initialise the objective
        /// </summary>
        public virtual void SetUpObjective()
        {
            QuestEvents.OnLoadNewScene += OnLoadNewScene;
            UpdatedCompletion = false;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Clear quest data and advance to the next stage once the objective is complete.
        /// </summary>
        protected virtual void UpdateCompleted()
        {
            if (UpdatedCompletion) return;
            UpdatedCompletion = true;
            DisableQuestMarkers();
            QuestSystem.instance.UpdateQuests();
            AudioManager.instance.PlayObjectiveComplete();
            QuestEvents.ObjectiveAdvance();
        }

        /// <summary>
        /// Display the quest markers
        /// </summary>
        /// <param name="pos">Position to display quest markers</param>
        protected void SpawnQuestMarker(Vector3 pos)
        {
            if (StaticLevel.enableQuestMarkers == false) return;

            GameObject gO = LoadQuestMarker();
            if (gO != null)
            {
                gO.transform.position = pos;
            }
        }

        protected virtual void OnLoadNewScene()
        {
            QuestEvents.OnLoadNewScene -= OnLoadNewScene;
        }
        #endregion

        #region Private Methods
        private GameObject LoadQuestMarker()
        {
            GameObject gO = Instantiate(Resources.Load<GameObject>("Quests/QuestMarker/" + questMarker_slug));
            if (gO != null)
                questMarkers.Add(gO);
            return gO;
        }
        #endregion

        #region Abstract Methods
        public abstract void EnableQuestMarkers();

        public abstract bool CheckCompleted();
        #endregion


    }
}