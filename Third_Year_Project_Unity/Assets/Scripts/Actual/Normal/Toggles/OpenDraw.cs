using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Triggers
{
    //open and close draws
    public class OpenDraw : ToggleBase
    {
        private Vector3 limitsMin;
        private Vector3 limitsMax;

        /// <summary>
        /// Set up clamp limits based on child positions
        /// </summary>
        void Start()
        {
            limitsMin = transform.GetChild(0).position;
            limitsMax = transform.GetChild(1).position;
            //ensure min is the smaller value
            if(limitsMin.x > limitsMax.x)
            {
                float l = limitsMin.x;
                limitsMin.x = limitsMax.x;
                limitsMax.x = l;
            }
            if (limitsMin.y > limitsMax.y)
            {
                float l = limitsMin.y;
                limitsMin.y = limitsMax.y;
                limitsMax.y = l;
            }
            if (limitsMin.z > limitsMax.z)
            {
                float l = limitsMin.z;
                limitsMin.z = limitsMax.z;
                limitsMax.z = l;
            }
        }


        protected override void NotInteracting()
        {
            
        }

        /// <summary>
        /// When the handle is pulled, make the draw follow the hand to the limits
        /// </summary>
        protected override void OnInteracting(OVRGrabber grabber)
        {
            Vector3 targetPos = grabber.transform.position;
            targetPos.x = Mathf.Clamp(targetPos.x, limitsMin.x, limitsMax.x);
            targetPos.y = Mathf.Clamp(targetPos.y, limitsMin.y, limitsMax.y);
            targetPos.z = Mathf.Clamp(targetPos.z, limitsMin.z, limitsMax.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10);
        }
    }
}