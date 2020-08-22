using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Menu
{
    public class ObjectivesMenu : MonoBehaviour
    {
        private GameObject objective;

        private void Start()
        {
            objective = transform.GetChild(0).gameObject;
            objective.SetActive(false);
        }

        private void Update()
        {
            if (InputManager.instance.OpenJournelButton())
            {
                objective.SetActive(!objective.activeSelf);
            }
        }
    }
}