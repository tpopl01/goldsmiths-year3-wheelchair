using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using tpopl001.Questing;
using tpopl001.Audio;

namespace tpopl001.Triggers
{
    public class ButtonPress : ToggleBase
    {
        private bool buttonPressed;

        private Transform button;

        [Space]
        [Header("Button Settings")]
        [SerializeField] private float buttonPressSpeed = 0.1f;
        [SerializeField] private float clampButtonPress = -0.0189f;
        [SerializeField] private int id = -1;

        [Space]
        [Header("Event")]
        [SerializeField] private UnityEvent m_MyEvent = new UnityEvent();

        public int GetID()
        {
            return id;
        }

        void Start()
        {
            button = transform.GetChild(0);
        }

        protected override void NotInteracting()
        {
            //return button to start position
            if (button.localPosition.z < 0)
                button.localPosition += Vector3.forward * Time.deltaTime * buttonPressSpeed;
            else
                buttonPressed = false;
        }

        protected override void OnInteracting(OVRGrabber grabber)
        {
            //update button visuals and call press
            if (button.localPosition.z > clampButtonPress)
            {
                button.localPosition -= Vector3.forward * Time.deltaTime * buttonPressSpeed;
                PressButton();
            }
        }

        public void PressButton()
        {
            if (buttonPressed)
                return;
            buttonPressed = true;
            if (m_MyEvent != null)
                m_MyEvent.Invoke();

            AudioManager.instance.PlayButton();
            //Call event to check button ID
            QuestEvents.ButtonPress(id);
        }

    }
}