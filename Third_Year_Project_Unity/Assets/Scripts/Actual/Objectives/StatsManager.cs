using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using tpopl001.JSON;

namespace tpopl001.Questing
{
    public class StatsManager : MonoBehaviour
    {
        private Text statsText;
        private GameObject displayBoard;
        [SerializeField] private string load_scene = "";
        [SerializeField] Image statsImage;

        #region Initialise
        public static StatsManager instance;
        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            displayBoard = transform.GetChild(0).gameObject;
            displayBoard.SetActive(false);
            statsText = displayBoard.GetComponentInChildren<Text>();
        }
        #endregion

        /// <summary>
        /// Get the scene to load
        /// </summary>
        public string GetScene()
        {
            return load_scene;
        }

        /// <summary>
        /// Load the next scene
        /// </summary>
        public void NextScene()
        {
            if(string.IsNullOrEmpty(load_scene))
            {
                Debug.LogWarning("Assign scene to load next.");
                return;
            }

            QuestEvents.LoadNewScene();
            SceneManager.LoadScene(load_scene);
        }

        /// <summary>
        /// Display the users level stats on the UI
        /// </summary>
        public void DisplayStats(Stats stats)
        {
            displayBoard.SetActive(true);
            statsText.text = stats.GetMedal();
            statsImage.sprite = Resources.Load<Sprite>("Quests/trophy/" + stats.GetImage());
        }

    }
}