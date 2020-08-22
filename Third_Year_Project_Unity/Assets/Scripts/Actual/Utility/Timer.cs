using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using tpopl001.Questing;

namespace tpopl001.Util {
    //While the quest is active record the time it takes
    public class Timer : MonoBehaviour
    {
        float timer = 0;
        [SerializeField]Text timerText = null;
        bool timerRunning = true;


        void OnDestroy()
        {
            QuestEvents.OnQuestComplete -= StopTimer;
        }

        void Start()
        {
            QuestEvents.OnQuestComplete += StopTimer;
        }

        void Update()
        {
            if (timerRunning)
            {
                timer += Time.deltaTime;
                timerText.text = "Time: " + Mathf.FloorToInt(timer).ToString();
            }
        }

        void StopTimer()
        {
            timerRunning = false;
        }
    }
}
