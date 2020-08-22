using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Menu
{
    //simple class to quit the game to desktop when a button is pressed
    public class Quit : MonoBehaviour
    {
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}