using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Audio;

namespace tpopl001.Questing
{
    [CreateAssetMenu(menuName = "Quests/Objectives/Collection")]
    public class CollectionObjective : Objective
    {
        [SerializeField] private CollectionItem[] collectionItems = null;

        #region Clean up

        protected override void OnLoadNewScene()
        {
            base.OnLoadNewScene();
            QuestEvents.OnItemCollected -= ItemTaken;
        }
        void OnDestroy()
        {
            QuestEvents.OnItemCollected -= ItemTaken;
        }
        #endregion

        #region Overrides
        public override string GetDescription()
        {
            return base.GetDescription() + " You have collected: " + GetAllCollected();
        }

        //Text to print to UI of collection amount
        private string GetAllCollected()
        {
            string retVal = "";
            for (int i = 0; i < collectionItems.Length; i++)
            {
                if (string.IsNullOrEmpty(retVal))
                    retVal += collectionItems[i].PrintLn();
                else
                    retVal += ", " + collectionItems[i].PrintLn();
            }
            return retVal;
        }

        //subscribe to collect event
        //spawn collectibles
        public override void SetUpObjective()
        {
            base.SetUpObjective();
            QuestEvents.OnItemCollected += ItemTaken;
            for (int i = 0; i < collectionItems.Length; i++)
            {
                collectionItems[i].SpawnItems();

            }
            EnableQuestMarkers();

        }

        public override bool CheckCompleted()
        {
            for (int i = 0; i < collectionItems.Length; i++)
            {
                if (collectionItems[i].AreAllItemsCollected() == false)
                    return false;
            }
            return true;
        }

        protected override void UpdateCompleted()
        {
            QuestEvents.OnItemCollected -= ItemTaken;
            base.UpdateCompleted();
        }
      
        public override void EnableQuestMarkers()
        {
            //enable quest markers at each collectible location
            for (int i = 0; i < collectionItems.Length; i++)
            {
                for (int x = 0; x < collectionItems[i].GetPos().Length; x++)
                {
                    SpawnQuestMarker(collectionItems[i].GetPos()[x]);
                }

            }
            SpawnQuestMarker(GetObjectivePosition());
        }
        #endregion

        /// <summary>
        /// Checks if the picked up item is the correct item.
        /// Plays audio and updates objective accordingly
        /// </summary>
        /// <param name="slug">The slug of the picked up object</param>
        private void ItemTaken(string slug)
        {
            for (int i = 0; i < collectionItems.Length; i++)
            {
                if (collectionItems[i].CollectItem(slug))
                {
                    AudioManager.instance.PlayObjectiveComplete();
                    break;
                }
            }
            if (CheckCompleted())
            {
                UpdateCompleted();
            }
            QuestEvents.ObjectiveAdvance();
        }

    }

    [System.Serializable]
    public class CollectionItem
    {
        private int collected = 0;
        [SerializeField] private string name = "";
        [SerializeField] private string object_slug = "";
        [SerializeField] private Vector3[] positions = null;

        public Vector3[] GetPos()
        {
            return positions;
        }

        /// <summary>
        /// Checks if the picked up item is the correct item.
        /// Increases the progress
        /// </summary>
        /// <param name="slug">The slug of the picked up object</param>
        public bool CollectItem(string slug)
        {
            if (slug.Equals(object_slug))
            {
                if (collected < positions.Length)
                {
                    collected++;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Has the user collected all the items
        /// </summary>
        public bool AreAllItemsCollected()
        {
            return collected == positions.Length;
        }

        /// <summary>
        /// Creates and spawns the collectible items in the scene
        /// </summary>
        public void SpawnItems()
        {
            collected = 0;
            for (int i = 0; i < positions.Length; i++)
            {
                GameObject go = Object.Instantiate(Resources.Load<GameObject>("Quests/Collectibles/" + object_slug));
                go.transform.position = positions[i];
            }
        }

        public string PrintLn()
        {
            return name + ": " + collected + " / " + positions.Length;
        }
    }
}