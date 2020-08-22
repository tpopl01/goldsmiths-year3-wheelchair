using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Util
{
    //An object to create a timer with
    public class CountDownTimer
    {
        private float startTime;
        public float Duration { get; set; }

        public CountDownTimer(float duration)
        {
            this.Duration = duration;
        }

        /// <summary>
        /// Begin the timer
        /// </summary>
        public void StartTimer()
        {
            startTime = GetTime();
        }

        /// <summary>
        /// Is the timer finished
        /// </summary>
        public bool GetComplete()
        {
            return GetTime() >= startTime + Duration;
        }

        /// <summary>
        /// The current game time
        /// </summary>
        private float GetTime()
        {
            return Time.time;
        }
    }
}