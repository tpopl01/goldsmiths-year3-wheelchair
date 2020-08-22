using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Questing;

namespace tpopl001.Triggers {
    public class ButtonTrigger : MonoBehaviour
    {
        private ButtonPress buttonPress;

        void Start()
        {
            //disable if not in no hands mode
            if (StaticLevel.noHands == false)
            {
                gameObject.SetActive(false);
                return;
            }

            buttonPress = GetComponentInParent<ButtonPress>();
        }

        /// <summary>
        /// Force press the button when close if it is part of the objective
        /// </summary>
        void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                //Get Current Objective
                ButtonObjective obj = QuestSystem.instance.GetCurrentObjective() as ButtonObjective;
                Debug.Log(obj);
                if (obj)
                {
                    //check objective involves this button
                    if (obj.GetButtonID() == buttonPress.GetID())
                    {
                        //if it does then press button
                        buttonPress.PressButton();
                    }
                }
            }
        }
    }
}