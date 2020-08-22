using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace tpopl001.Menu
{
    public class LevelPrefab : MonoBehaviour
    {
        private string levelName;
        private string levelTitle;
       // private string objectiveTitle;

        [SerializeField] private Text levelNameText = null;
        [SerializeField] private Text levelTitleText = null;

        /// <summary>
        /// Set the scripts variables
        /// </summary>
        /// <param name="levelName">The level to load</param>
        /// <param name="levelTitle">The quest to load</param>
        /// <param name="objectiveTitle">The objective to load</param>
        public void Initialise(string levelName, string levelTitle)
        {
            this.levelName = levelName;
            this.levelTitle = levelTitle;
            levelNameText.text = levelName;
            levelTitleText.text = levelTitle;
        }

        /// <summary>
        /// Set the global variables and loads the level.
        /// This variables are used to initialise the quest at the desired point.
        /// </summary>
        public void LoadLevel()
        {
            StaticLevel.levelTitle = levelTitle;
            StaticLevel.levelName = levelName;
        //    StaticLevel.objectiveTitle = objectiveTitle;

            SceneManager.LoadScene(levelName);
        }
    }
}