using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using tpopl001.JSON;

namespace tpopl001.Questing
{
    [CreateAssetMenu(menuName = "Quests/Quest/Basic Quest")]
    public class Quest : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private string title = "";
        [SerializeField] [TextArea] private string description = "";

        [Space]
        [Header("Objectives")]
        [SerializeField]private Objective[] objectives = null;
        private Stats[] stats;
        private bool complete = false;
        private int startProgress = 0;

        public int Progress { get; private set; } = 0;

        #region Public Methods
        public string GetTitle()
        {
            return title;
        }

        public string GetDescription()
        {
            return description;
        }

        /// <summary>
        /// Set the objective to the objective the user has loaded
        /// </summary>
        public void LoadLevel()
        {
            if (string.IsNullOrEmpty(StaticLevel.objectiveTitle))
            {
                Progress = 0;
                return;
            }
            for (int i = 0; i < objectives.Length; i++)
            {
                if (objectives[i].GetTitle().Equals(StaticLevel.objectiveTitle))
                {
                    Progress = i;
                }
            }
        }

        /// <summary>
        /// Begin the quest at the loaded objective
        /// </summary>
        public void StartQuest()
        {
            startProgress = Progress;
            SetupStats();
            complete = false;
            if (Progress < objectives.Length)
            {
                SetUpNextObj();
            }
        }

        /// <summary>
        /// Gets the objectives description
        /// </summary>
        public string GetObjectiveDesc()
        {
            string retVal = "";
            if (Progress < objectives.Length)
                retVal = objectives[Progress].GetDescription();
            return retVal;
        }

        /// <summary>
        /// Tells the objective to enable or disable quest markers
        /// </summary>
        public void DisableQuestMarkers(bool disable)
        {
            if (disable) objectives[Progress].DisableQuestMarkers();
            else objectives[Progress].EnableQuestMarkers();
        }

        /// <summary>
        /// Returns the current in-progress objective
        /// </summary>
        public Objective GetCurrentObjective()
        {
            if (Progress > objectives.Length - 1)
                return null;
            return objectives[Progress];
        }

        /// <summary>
        /// Add a stat to the objective
        /// </summary>
        public void AddToStat(string statName)
        {
            for (int i = 0; i < stats.Length; i++)
            {
                if (stats[i].GetStatName().Equals(statName))
                {
                    stats[i].IncreasePoints();
                    break;
                }
            }
        }

        /// <summary>
        /// Have we completed the current objective. If we have then advance to the next objective
        /// </summary>
        public bool IsCurrentObjectiveComplete()
        {
            if (complete == true) return true;
            if (objectives[Progress].CheckCompleted())
                AdvanceStage();

            return complete;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get the stats associated with the objective.
        /// </summary>
        private void SetupStats()
        {
            stats = new Stats[1];
            stats[0] = new Stats("Obstacles Hit", 0);
        }

        /// <summary>
        /// Advance to the next objective if available, 
        /// otherwise mark the quest as complete
        /// </summary>
        private void AdvanceStage()
        {
            if (Progress < objectives.Length - 1)
            {
                Progress++;
                SetUpNextObj();
            }
            else
                Complete();
        }

        private void SetUpNextObj()
        {
            objectives[Progress].SetUpObjective();
        }

        /// <summary>
        /// Start the quest complete event,
        /// Display the stats board
        /// Save new stats to JSON
        /// </summary>
        private void Complete()
        {
            QuestEvents.QuestComplete();
            complete = true;
            StatsManager.instance.DisplayStats(stats[0]);
            SaveObjectives();
        }

        /// <summary>
        /// Save all the newly completed objectives to JSON
        /// </summary>
        private void SaveObjectives()
        {
            if (string.IsNullOrEmpty(StaticLevel.username)) return;
            for (int i = startProgress; i < objectives.Length; i++)
            {
                if(!string.IsNullOrEmpty(objectives[i].GetTitle()))
                    UserJson.ModifyLevelJson(StaticLevel.username, new Level(SceneManager.GetActiveScene().name, title, stats));
            }
        }
        #endregion


    }

    public class ObjectiveStats
    {
        public string objectiveTitle;
        public Stats[] stats;

        public ObjectiveStats(string objective_name, Stats[] stats)
        {
            this.objectiveTitle = objective_name;
            this.stats = stats;
        }

        public void AddToStat(string stat_name)
        {
            for (int i = 0; i < stats.Length; i++)
            {
                if(stat_name.Equals(stats[i].GetStatName()))
                {
                    stats[i].IncreasePoints();
                    break;
                }
            }
        }
    }

    [System.Serializable]
    public class StatsData
    {
        public string stat_name;
        public int points;

        public void Increment(int amount)
        {
            points += amount;
        }

        public StatsData(string stat_name, int points)
        {
            this.stat_name = stat_name;
            this.points = points;
        }
    }

    [System.Serializable]
    public class Stats
    {
        public string stat_name;
        public int points;
        public Stats(string stat_name, int points)
        {
            this.stat_name = stat_name;
            this.points = points;
        }
        public string GetStatName()
        {
            return stat_name;
        }

        public void IncreasePoints()
        {
            points++;
        }

        public string GetImage()
        {
            if (points <= 0 && points >= 0)
                return "trophy_perfect";
            if (points <= 15 && points >= 1)
                return "trophy_gold";
            if (points <= 50 && points >= 16)
                return "trophy_silver";
            return "trophy_bronze";
        }

        public string GetMedal()
        {
            string prev = "";
            if(!string.IsNullOrEmpty(StaticLevel.username))
            {
                Level l = JSON.UserJson.GetUserLevelData(StaticLevel.username, StaticLevel.levelName, StaticLevel.levelTitle);
                if(l!= null)
                {
                    for (int i = 0; i < l.stats.Length; i++)
                    {
                        if(l.stats[i].GetStatName().Equals(stat_name))
                        {
                            prev = ", An improvement of (" + (l.stats[i].points- points) + ")";
                            break;
                        }
                    }
                }
            }

            if (points <= 0 && points >= 0)
                return stat_name + ": Perfect " + points + prev;
            if (points <= 15 && points >= 1)
                return stat_name + ": Gold " + points + prev;
            if (points <= 50 && points >= 16)
                return stat_name + ": Silver " + points + prev;
            return stat_name + ": Bronze " + points + prev;
        }
    }
}