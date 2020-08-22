using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Menu
{
    //utility class that is used on button events to enable or disable a group of objects
    public class SetActive : MonoBehaviour
    {
        [SerializeField] bool setActive = false;
        [SerializeField] GameObject[] activationObjects = null;

        public void ChangeActivation()
        {
            for (int i = 0; i < activationObjects.Length; i++)
            {
                activationObjects[i].SetActive(setActive);
            }
        }
    }
}