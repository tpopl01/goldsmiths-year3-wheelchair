using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace tpopl001.Menu
{
    //simple class that handles displaying the welcome message
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Text welcomeMessage = null;

        private void Start()
        {
            UpdateWelcomeText();
        }

        /// <summary>
        /// Updates the welcome text to reflect whether or not a user profile is active
        /// </summary>
        public void UpdateWelcomeText()
        {
            if (string.IsNullOrEmpty(StaticLevel.username) == false)
                welcomeMessage.text = "Welcome " + StaticLevel.username;
            else
                welcomeMessage.text = "Welcome Guest";
        }
    }
}
