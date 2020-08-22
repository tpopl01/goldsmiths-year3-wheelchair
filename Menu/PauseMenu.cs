using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        GameObject menu;
        bool menuOpen = false;
        PlayerSettings playerSettings;

        #region initialise
        public static PauseMenu instance;
        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            playerSettings = Resources.Load<PlayerSettings>("Settings/PlayerSettings");
            //  hmd = GameObject.FindObjectOfType<OVRCameraRig>().transform;
            menu = transform.GetChild(0).GetChild(1).gameObject;
            Resume();
        }
        #endregion

        /// <summary>
        /// Pause or resume based on input
        /// </summary>
        void Update()
        {
            if (InputManager.instance.PauseButton())
            {
                if (!menuOpen)
                    Pause();
                else
                    Resume();
            }
        }

        /// <summary>
        /// Called by event to close this menu when sub menus are opened
        /// </summary>
        public void CloseMenu()
        {
            menu.SetActive(false);
        }

        /// <summary>
        /// Called by event to open the menu when sub menus are closed
        /// </summary>
        public void OpenMenu()
        {
            menu.SetActive(true);
        }

        /// <summary>
        /// Pause the game
        /// </summary>
        public void Pause()
        {
            menuOpen = true;
            transform.GetChild(0).gameObject.SetActive(true);
            Settings.instance.CloseSettings();
            OpenMenu();
            Vector3 pos = playerSettings.HMD.position;
            transform.LookAt(pos);
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
        }

        /// <summary>
        /// Close the pause menu
        /// </summary>
        public void Resume()
        {
            menuOpen = false;
            transform.GetChild(0).gameObject.SetActive(false);
            Settings.instance.CloseSettings();
        }
    }
}