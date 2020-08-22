using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Audio;

namespace tpopl001.Questing
{
    [RequireComponent(typeof(Collider))]
    public class CollectionTrigger : MonoBehaviour
    {
        [Header("Slugs")][Tooltip("Only accept items with these names")]
        [SerializeField]string[] acceptedItemSlugs = new string[0];

        void OnTriggerEnter(Collider col)
        {
            Process(col);
        }

        /// <summary>
        /// Passes the name of the collided object to an event.
        /// An objective can then use this to detect if the correct item has been picked up
        /// </summary>
        protected virtual void Process(Collider col)
        {
            string colname = StaticCalculations.ProcessObjectName(col.name);
            if (acceptedItemSlugs.Length == 0)
            {
                QuestEvents.ItemCollected(colname);
            }
            else
            {
                for (int i = 0; i < acceptedItemSlugs.Length; i++)
                {
                    if (colname.Equals(acceptedItemSlugs[i]))
                    {
                        QuestEvents.ItemCollected(colname);
                        AudioManager.instance.PlayObjectiveComplete();
                        break;
                    }
                }
            }
        }
    }
}