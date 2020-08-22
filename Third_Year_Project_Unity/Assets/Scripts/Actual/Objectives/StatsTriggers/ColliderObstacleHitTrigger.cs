using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Questing
{
    //Modify an objective stat on collision
    public class ColliderObstacleHitTrigger : MonoBehaviour
    {
        #region Variables
        [Header("Tag")]
        [SerializeField] private string t = "Player";
        [Space]
        [Header("Stat To Modify")]
        [SerializeField] private const string stat = "Obstacles Hit";
        [Space]
        [Header("References")]
        [SerializeField] private AudioSource aS = null;
        [SerializeField] private AudioClip aC = null;
        #endregion

        //when there is a collision with the player modify the stat and inform the user via audio
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals(t))
            {
                QuestSystem.instance.AddStat(stat);
                Audio.AudioManager.instance.PlayOneShot(aS, aC);
            }
        }
    }
}