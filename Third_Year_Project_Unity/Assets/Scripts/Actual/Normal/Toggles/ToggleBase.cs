using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Triggers
{
    public abstract class ToggleBase : MonoBehaviour
    {
        [SerializeField] private float maxHandDist = 0.2f;
        PlayerSettings playerSettings;

        private void Awake()
        {
            playerSettings = Resources.Load<PlayerSettings>("Settings/PlayerSettings");
        }

        //handle detection of interactions and calling of relevant functions
        void Update()
        {
            for (int i = 0; i < playerSettings.Hands.Length; i++)
            {
                //if our hand is grabbing and near the door, then interact
                OVRGrabber grabber = playerSettings.Hands[i];
                if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, grabber.GetController()) != 0 && Vector3.Distance(grabber.transform.position, transform.position) < maxHandDist)
                {
                    OnInteracting(grabber);
                    return;
                }
            }

            NotInteracting();
        }

        protected abstract void OnInteracting(OVRGrabber grabber);

        protected abstract void NotInteracting();

    }
}