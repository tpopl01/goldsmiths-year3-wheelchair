using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the playing of all 2D audio such as sound effects. 
//Is a local spot for specific audio clips that are called from multiple locations.
//This way I only need to change the audio clip here if I want to update it.
namespace tpopl001.Audio
{
    public class AudioManager : MonoBehaviour
    {
        #region Variables
        #region Public variables
        [Header("Global Audio Source")]
        [SerializeField] AudioSource global2DAudioSource = null;
        #endregion

        #region Private Variables
        private AudioC buttonAudio = null;
        private AudioC collisionAudio = null;
        private AudioC completeAudio = null;
        #endregion
        #endregion

        #region Initialisation
        #region Singleton
        public static AudioManager instance;
        void Awake()
        {
            instance = this;
        }
        #endregion

        private void Start()
        {
            CreateAudioObjects();
        }

        /// <summary>
        /// Initialise the audio objects to a desired volume and clip
        /// </summary>
        private void CreateAudioObjects()
        {
            buttonAudio = new AudioC(Resources.Load<AudioClip>("Audio/Effects/Button Click"), 1);
            collisionAudio = new AudioC(Resources.Load<AudioClip>("Audio/Effects/Collision"), 1);
            completeAudio = new AudioC(Resources.Load<AudioClip>("Audio/Effects/objective complete 1"), 0.6f);
        }
        #endregion

        #region Play Clip
        #region Play General Audio
        /// <summary>
        /// Play an audio clip on a given audio source
        /// </summary>
        public void PlayOneShot(AudioSource aS, AudioClip aC)
        {
            if (aS.isPlaying == false)
                aS.PlayOneShot(aC);
        }
        /// <summary>
        /// Play an audio clip on the global audio source
        /// </summary>
        public void PlayOneShot(AudioClip aC)
        {
            if (global2DAudioSource.isPlaying == false)
            {
                global2DAudioSource.volume = 1;
                global2DAudioSource.PlayOneShot(aC);
            }
        }
        #endregion

        #region Play Button Audio
        /// <summary>
        /// Play button press on a given audio source
        /// </summary>
        public void PlayButton(AudioSource aS)
        {
            if (!aS.isPlaying)
                aS.PlayOneShot(buttonAudio.AudioClip);
        }
        /// <summary>
        /// Play button press on the global audio source
        /// </summary>
        public void PlayButton()
        {
            GlobalPlay(buttonAudio);
        }
        #endregion

        #region Play Collision Audio
        /// <summary>
        /// Play collision on a given audio source
        /// </summary>
        public void PlayCollision(AudioSource aS)
        {
            if (!aS.isPlaying)
                aS.PlayOneShot(collisionAudio.AudioClip);
        }
        /// <summary>
        /// Play collision on the global audio source
        /// </summary>
        public void PlayCollision()
        {
            GlobalPlay(collisionAudio);
        }
        #endregion

        #region Play Objective Complete Audio
        /// <summary>
        /// Play Objective Complete on a given audio source
        /// </summary>
        public void PlayObjectiveComplete(AudioSource aS)
        {
            if (!aS.isPlaying)
                aS.PlayOneShot(completeAudio.AudioClip);
        }
        /// <summary>
        /// Play Objective Complete on the global audio source
        /// </summary>
        public void PlayObjectiveComplete()
        {
            GlobalPlay(completeAudio);
        }
        #endregion

        /// <summary>
        /// Play the Audio object to the global audiosource.
        /// Adjust volume accordingly
        /// </summary>
        void GlobalPlay(AudioC audioC)
        {
            if (!global2DAudioSource.isPlaying)
            {
                global2DAudioSource.volume = audioC.Volume;
                global2DAudioSource.PlayOneShot(audioC.AudioClip);
            }
        }
        #endregion
    }

    //simple audio object to have a volume option for each clip
    public class AudioC
    {
        public AudioClip AudioClip { get; private set; }
        public float Volume { get; private set; } = 1;

        public AudioC(AudioClip aC, float volume)
        {
            this.AudioClip = aC;
            this.Volume = Mathf.Clamp01(volume);
        }
    }
}