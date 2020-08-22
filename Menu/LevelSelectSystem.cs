using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.JSON;

namespace tpopl001.Menu
{
    //manages displaying levels dynamically in the UI
    public class LevelSelectSystem : MonoBehaviour
    {
        private string[] levelNames = new string[] { "Level_01", "Level_02", "Level_03", "Level_04" };
        [SerializeField] private Transform prefabContainer = null;
        private const string LEVEL_PREFAB = "level_prefab";

        #region Init
        public static LevelSelectSystem instance;
        private void Awake()
        {
            instance = this;
        }
        #endregion

        #region Utility
        /// <summary>
        /// Remove gameObjects that are achild of prefabContainer
        /// </summary>
        private void DestroyLevelPrefabs()
        {
            for (int i = 0; i < prefabContainer.childCount; i++)
            {
                Destroy(prefabContainer.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Get the JSON user
        /// </summary>
        /// <param name="username">The user you want to retrieve</param>
        private JsonUser GetUser(string username)
        {
            if (!string.IsNullOrEmpty(username))
                return UserJson.GetUser(username);
            return null;
        }
        #endregion

        #region Display Levels
        /// <summary>
        /// Function to determine what level data to display.
        /// It is called whenever the level select UI panel is opened
        /// </summary>
        public void DisplayLevels()
        {
            DestroyLevelPrefabs();
            JsonUser user = GetUser(StaticLevel.username);
            if (user != null)
            {
                //Display unlocked levels
                DisplayUserSpecificLevels(user);
            }
            else
            {
                //Display all levels
                DisplayDefaultLevels();
            }
        }

        /// <summary>
        /// Display the levels the user has unlocked.
        /// </summary>
        private void DisplayUserSpecificLevels(JsonUser user)
        {
            for (int i = 0; i < user.Levels.Length; i++)
            {
                DisplayLevel(user.Levels[i].LevelName, user.Levels[i].LevelTitle);
            }
        }

        /// <summary>
        /// Display all the levels
        /// </summary>
        private void DisplayDefaultLevels()
        {
            for (int i = 0; i < levelNames.Length; i++)
            {
                DisplayLevel(levelNames[i], "");
            }
        }

        /// <summary>
        /// Code to instantiate a level prefab and display a level
        /// </summary>
        private void DisplayLevel(string levelName, string levelTitle)
        {
            LevelPrefab lP = Instantiate<LevelPrefab>(Resources.Load<LevelPrefab>("Menu/" + LEVEL_PREFAB));
            lP.Initialise(levelName, levelTitle);
            lP.transform.SetParent(this.prefabContainer);
            RectTransform rT = lP.GetComponent<RectTransform>();
            rT.localPosition = new Vector3(rT.localPosition.x, rT.localPosition.y, 0);
            rT.localScale = Vector3.one;
        }
        #endregion
    }
}