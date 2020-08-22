using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace tpopl001.Questing
{
    // A singleton class to handle quests
    public class QuestSystem : MonoBehaviour
    {
        public List<Quest> quests = new List<Quest>();
        [SerializeField] private Text questTitleText = null;
        [SerializeField] private Text questDescriptionText = null;
        [SerializeField] private Text[] objectiveDescriptionText = null;
        private int currentQuest = 0;

        #region Built-in Methods
        private void OnDestroy()
        {
            QuestEvents.OnObjectiveAdvance -= UpdateObjectiveUI;
        }

        private void Start()
        {
            LoadLevel();
            QuestEvents.OnObjectiveAdvance += UpdateObjectiveUI;
            if(quests.Count > currentQuest)
            {
                StartQuest();
            }

            StaticLevel.levelName = SceneManager.GetActiveScene().name;
            StaticLevel.levelTitle = quests[currentQuest].GetTitle();
        }

        //update the quest event tick every frame
        private void Update()
        {
            QuestEvents.Tick();
        }
        #endregion

        /// <summary>
        /// Set the objective to the objective the user has loaded
        /// </summary>
        private void LoadLevel()
        {
            quests[currentQuest].LoadLevel();
        }

        /// <summary>
        /// Update the UI to reflect the objective
        /// </summary>
        private void UpdateObjectiveUI()
        {
            if (quests.Count > currentQuest)
                DisplayQuestObjectives(quests[currentQuest]);
        }

        /// <summary>
        /// Start the quest
        /// </summary>
        private void StartQuest()
        {
            quests[currentQuest].StartQuest();
            DisplayQuestObjectives(quests[currentQuest]);
        }

        /// <summary>
        /// Try to update to the next quest
        /// </summary>
        public void UpdateQuests()
        {
            if(quests[currentQuest].IsCurrentObjectiveComplete())
            {
                if (currentQuest < quests.Count)
                {
                    currentQuest++;
                }
                if (currentQuest >= quests.Count)
                {
                    questTitleText.text = "Quest Complete!";
                    questDescriptionText.text = "";
                    for (int i = 0; i < objectiveDescriptionText.Length; i++)
                    {
                        objectiveDescriptionText[i].text = "Head over to stats board. Press the button when you are ready to leave.";
                    }
                    
                }
                else
                {
                    StartQuest();
                }
            }
        }
        #region UI

        /// <summary>
        /// Update UI text fields to reflect the quest and objective titles
        /// </summary>
        private void ChangeQuestTitleDescription(Quest quest)
        {
            questTitleText.text = quest.GetTitle();
            questDescriptionText.text = quest.GetDescription();
            for (int i = 0; i < objectiveDescriptionText.Length; i++)
            {
                objectiveDescriptionText[i].text = quest.GetObjectiveDesc();
            }
            
        }

        public void DisplayQuestObjectives(Quest quest)
        {
            ChangeQuestTitleDescription(quest);
        }

        /// <summary>
        /// Method to enable or disable quest markers
        /// </summary>
        public void QuestMarkers(bool disable)
        {
            quests[currentQuest].DisableQuestMarkers(disable);
        }
        #endregion


        #region Utilities
        /// <summary>
        /// Method to add a stat value such as Obstacles Hit
        /// </summary>
        public void AddStat(string statName)
        {
            if(currentQuest < quests.Count)
                quests[currentQuest].AddToStat(statName);
        }

        /// <summary>
        /// Returns the current objective
        /// </summary>
        public Objective GetCurrentObjective()
        {
            if (currentQuest > quests.Count - 1)
                return null;

            return quests[currentQuest].GetCurrentObjective();
        }

        public static QuestSystem instance;
        private void Awake()
        {
            transform.SetParent(null);
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        #endregion
    }

    public class UIQuestObj
    {
        public bool isActive;
        public Quest quest;

        public UIQuestObj(Quest quest)
        {
            this.quest = quest;
        }
    }
}