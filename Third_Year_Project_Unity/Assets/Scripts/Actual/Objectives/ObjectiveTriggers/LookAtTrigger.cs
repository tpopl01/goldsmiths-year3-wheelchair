using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Questing
{
    public class LookAtTrigger : MonoBehaviour
    {
        public bool enableRay = false;
        public LayerMask layerMask;

        void Update()
        {
            //used in quests to fire a ray to detect what the user is looking at
            if (enableRay)
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 20, layerMask))
                {
                    QuestEvents.LookAt(hit);
                }
            }
        }
    }
}
