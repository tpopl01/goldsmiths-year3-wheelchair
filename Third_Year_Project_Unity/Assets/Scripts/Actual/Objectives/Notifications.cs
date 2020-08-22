using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace tpopl001.Notification
{
    public class Notifications : MonoBehaviour
    {

        List<NotificationObj> notificationQueue = new List<NotificationObj>();
        List<NotificationObj> mainNotificationQueue = new List<NotificationObj>();

        [SerializeField] Text notification1 = null;
        [SerializeField] Text notification2 = null;
        [SerializeField] Text notification3 = null;

        [SerializeField] Text mainNotification = null;

        private void Start()
        {
            notification1.gameObject.SetActive(false);
            notification2.gameObject.SetActive(false);
            notification3.gameObject.SetActive(false);
            mainNotification.gameObject.SetActive(false);
        }

        public void AddNotificationToQueue(NotificationObj n)
        {
            notificationQueue.Add(n);
        }

        public void AddMainNotificationToQueue(NotificationObj n)
        {
            mainNotificationQueue.Add(n);
        }

        private void Update()
        {
            if (notificationQueue.Count > 0)
            {
                if (!notificationQueue[0].inEffect)
                {
                    notification1.gameObject.SetActive(true);
                    notification1.text = notificationQueue[0].notification;
                    notificationQueue[0].inEffect = true;
                    StartCoroutine(DeactivateNotification(notificationQueue[0], notification1, notificationQueue));
                }
                if (notificationQueue.Count > 1)
                {
                    if (!notificationQueue[1].inEffect)
                    {
                        notification2.gameObject.SetActive(true);
                        notification2.text = notificationQueue[1].notification;
                        notificationQueue[1].inEffect = true;
                        StartCoroutine(DeactivateNotification(notificationQueue[1], notification2, notificationQueue));
                    }
                }
                if (notificationQueue.Count > 2)
                {
                    if (!notificationQueue[2].inEffect)
                    {
                        notification3.gameObject.SetActive(true);
                        notification3.text = notificationQueue[2].notification;
                        notificationQueue[2].inEffect = true;
                        StartCoroutine(DeactivateNotification(notificationQueue[2], notification3, notificationQueue));
                    }
                }
            }
            if (mainNotificationQueue.Count > 0)
            {
                if (!mainNotificationQueue[0].inEffect)
                {
                    mainNotification.gameObject.SetActive(true);
                    mainNotification.text = mainNotificationQueue[0].notification;
                    mainNotificationQueue[0].inEffect = true;
                    StartCoroutine(DeactivateNotification(mainNotificationQueue[0], mainNotification, mainNotificationQueue));
                }
            }
        }

        IEnumerator DeactivateNotification(NotificationObj nO, Text n, List<NotificationObj> queue)
        {
            yield return new WaitForSeconds(nO.timeVisible);
            n.gameObject.SetActive(false);
            queue.Remove(nO);
        }


        public static Notifications instance;
        private void Awake()
        {
            instance = this;
        }
    }

    public class NotificationObj
    {
        public float timeVisible;
        public string notification;
        public bool inEffect = false;

        public NotificationObj(float _timeVisible, string _notification)
        {
            timeVisible = _timeVisible;
            notification = _notification;
        }
    }
}