using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace tpopl001.Menu
{
    //script for menu events to load the next scene
    public class LoadScene : MonoBehaviour
    {
        [SerializeField] private string scenename = "";

        public void LoadNextScene()
        {
            Questing.QuestEvents.LoadNewScene();
            SceneManager.LoadScene(scenename);
        }
    }
}