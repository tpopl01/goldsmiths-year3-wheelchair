using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Questing
{
    public class LocationTrigger : MonoBehaviour
    {
        public int ticksBetweenUpdate = 2;
        private int ticks = 0;

        void Update()
        {
            //call the players location event every few frames
            ticks += 1;
            if(ticks > ticksBetweenUpdate)
            {
                ticks = 0;
                QuestEvents.LocationChange(transform.position);
            }
        }
    }
}