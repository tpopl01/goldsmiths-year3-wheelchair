using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        private AudioSource aS;
        private AudioClip[] clips = null;

        //initialise the variables
        void Start()
        {
            aS = GetComponent<AudioSource>();
            clips = Resources.LoadAll<AudioClip>("Audio/Music/");
        }

        //try to play a random music clip
        void Update()
        {
            PlayClip(clips[Random.Range(0, clips.Length)]);
        }

        /// <summary>
        /// Play an audio clip if one is not already playing
        /// </summary>
        /// /// <param name="clip">The clip to play</param>
        void PlayClip(AudioClip clip)
        {
            if (!aS.isPlaying)
            {
                aS.PlayOneShot(clip);
            }
        }
    }
}
