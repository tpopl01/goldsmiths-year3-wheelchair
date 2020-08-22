using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using tpopl001.Questing;
using UnityEngine.UI;

//class handles updating settings UI and adjusting setting variables in StaticLevel and playerPrefs text file
namespace tpopl001.Menu
{
    public class Settings : MonoBehaviour
    {
        #region Variables
        GameObject settings;

        #region Audio
        [Space]
        [Header("Audio")]
        public AudioMixer audioMixer;
        [SerializeField] Slider audioSlider = null;
        [SerializeField] Slider musicSlider = null;
        [SerializeField] Slider effectsSlider = null;
        #endregion

        #region Utility
        [Space]
        [Header("Utility")]
        [SerializeField] Slider speedSlider = null;
        [SerializeField] Toggle objectiveMarkersToggle = null;
        #endregion

        #region Comfort
        [Space]
        [Header("Comfort")]
        [SerializeField] Slider tunnelingSlider = null;
        [SerializeField] Slider stepMoveSlider = null;
        [SerializeField] Slider stepTurnSlider = null;
        [SerializeField] Toggle smoothMovementToggle = null;
        [SerializeField] Toggle tunnelingMove = null;
        [SerializeField] Toggle tunnelingTurn = null;
        [SerializeField] Toggle accelerationToggle = null;
        #endregion

        #region Usability
        [Space]
        [Header("Useability")]
        [SerializeField] Toggle leftHandToggle = null;
        [SerializeField] Toggle noStickToggle = null;
        [SerializeField] Toggle noHandsToggle = null;
        [SerializeField] bool allowNoHandsToggle = false;
        #endregion
        #endregion

       PlayerSettings playerSettings;

        #region Comfort
        private const string turnTunnelingString = "Turn_Tunneling";
        private const string moveTunnelingString = "Move_Tunneling";
        private const string amountTunnelingString = "Amount_Tunneling";

        private const string accelerationString = "Acceleration_On";

        private const string enableSmoothMovementString = "Allow_Smooth_Movement";

        private const string moveStepString = "Move_Step";
        private const string turnStepString = "Turn_Step";
        #endregion

        #region Audio
        private const string masterVolume = "Master_Volume";
        private const string musicVolume = "Music_Volume";
        private const string effectVolume = "Effect_Volume";
        #endregion

        #region Hands
        private const string noHandsString = "Use_No_Hands";
        private const string noStickString = "No_Joystick";
        private const string leftHandString = "Use_Left_Hand_Only";
        #endregion

        private const string maximumSpeed = "Max_Speed";
        private const string enableQuestMarkersString = "Quest_Markers_Enabled";

        #region Initialisation
        //Make class a singleton
        public static Settings instance;
        void Awake()
        {
            instance = this;
        }

        //Initialises the variables
        void Start()
        {
            playerSettings = Resources.Load<PlayerSettings>("Settings/PlayerSettings");
            settings = transform.GetChild(0).gameObject;
            InitialiseAudio();
            SetFromPlayerPrefs();
            SetUIVisuals();
            SetActivation();
            AdjustSceneToSettings();
            CloseSettings();
        }

        /// <summary>
        /// Gets settings from player prefs file and adjusts the sliders and volume accordingly	
        /// </summary>
        void InitialiseAudio()
        {
            float mV = PlayerPrefs.GetFloat(masterVolume);
            audioSlider.SetValueWithoutNotify(mV);
            float eV = PlayerPrefs.GetFloat(effectVolume);
            effectsSlider.SetValueWithoutNotify(eV);
            float muV = PlayerPrefs.GetFloat(musicVolume);
            musicSlider.SetValueWithoutNotify(muV);

            AdjustEffectsVolume(eV);
            AdjustMusicVolume(muV);
            AdjustVolume(mV);
        }

        /// <summary>
        /// Initialises the variables to the player prefs settings
        /// </summary>
        void SetFromPlayerPrefs()
        {
            StaticLevel.maxSpeed = PlayerPrefs.GetFloat(maximumSpeed);
            StaticLevel.leftHand = PlayerPrefs.GetInt(leftHandString) == 1;
            StaticLevel.noHands = PlayerPrefs.GetInt(noHandsString) == 1;
            StaticLevel.noStick = PlayerPrefs.GetInt(noStickString, 1) == 1;
            StaticLevel.enableQuestMarkers = PlayerPrefs.GetInt(enableQuestMarkersString) == 1;
            StaticLevel.accelerationTunnelVision = PlayerPrefs.GetInt(moveTunnelingString) == 1;
            StaticLevel.turningTunnelVision = PlayerPrefs.GetInt(turnTunnelingString) == 1;
            StaticLevel.tunnelVisionAmount = PlayerPrefs.GetFloat(amountTunnelingString);
            StaticLevel.acceleration = PlayerPrefs.GetInt(accelerationString) == 1;
            StaticLevel.movementStepAmount = PlayerPrefs.GetInt(moveStepString);
            StaticLevel.turnStepAmount = PlayerPrefs.GetInt(turnStepString);
            StaticLevel.smoothMovement = PlayerPrefs.GetInt(enableSmoothMovementString) == 1;
        }

        /// <summary>
        /// Updates the UI elements (toggles, sliders) to visually match the player prefs settings
        /// </summary>
        void SetUIVisuals()
        {
            speedSlider.SetValueWithoutNotify(StaticLevel.maxSpeed);
            leftHandToggle.SetIsOnWithoutNotify(StaticLevel.leftHand);
            noHandsToggle.SetIsOnWithoutNotify(StaticLevel.noHands);

            noStickToggle.SetIsOnWithoutNotify(StaticLevel.noStick);

            objectiveMarkersToggle.SetIsOnWithoutNotify(StaticLevel.enableQuestMarkers);

            tunnelingMove.SetIsOnWithoutNotify(StaticLevel.accelerationTunnelVision);
            tunnelingTurn.SetIsOnWithoutNotify(StaticLevel.turningTunnelVision);
            tunnelingSlider.SetValueWithoutNotify(StaticLevel.tunnelVisionAmount);

            accelerationToggle.SetIsOnWithoutNotify(StaticLevel.acceleration);
            stepMoveSlider.SetValueWithoutNotify(StaticLevel.movementStepAmount);
            stepTurnSlider.SetValueWithoutNotify(StaticLevel.turnStepAmount);

            smoothMovementToggle.SetIsOnWithoutNotify(StaticLevel.smoothMovement);
        }

        /// <summary>
        /// Updates the scene to  reflect the chosen setting state
        /// </summary>
        void AdjustSceneToSettings()
        {
            Speed(StaticLevel.maxSpeed);
            NoWheelchairStick(StaticLevel.noStick);
            NoHands(StaticLevel.noHands);
            ObjectiveMarkers(StaticLevel.enableQuestMarkers);
        }

        #endregion

        #region Utility
        /// <summary>
        /// Handles showing and hiding options depending on what settings are selected	
        /// </summary>
        void SetActivation()
        {
            if (StaticLevel.noStick)
            {
                leftHandToggle.gameObject.SetActive(false);
                noHandsToggle.gameObject.SetActive(false);
                noStickToggle.gameObject.SetActive(true);
            }
            else if (StaticLevel.noHands)
            {
                leftHandToggle.gameObject.SetActive(false);
                noStickToggle.gameObject.SetActive(false);
                if (allowNoHandsToggle) noHandsToggle.gameObject.SetActive(true);
            }
            else
            {
                leftHandToggle.gameObject.SetActive(true);
                noStickToggle.gameObject.SetActive(true);
                if (allowNoHandsToggle)
                    noHandsToggle.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Open settings UI
        /// </summary>
        public void OpenSettings()
        {
            settings.SetActive(true);
        }

        /// <summary>
        /// Close Settings UI
        /// </summary>
        public void CloseSettings()
        {
            settings.SetActive(false);
        }
        #endregion

        #region Events
        public void AdjustVolume(float volume)
        {
            PlayerPrefs.SetFloat(masterVolume, volume);
            audioMixer.SetFloat("volume_master", volume);

        }

        public void AdjustMusicVolume(float volume)
        {
            PlayerPrefs.SetFloat(musicVolume, volume);
            audioMixer.SetFloat("volume_music", volume);
        }

        public void AdjustEffectsVolume(float volume)
        {
            PlayerPrefs.SetFloat(effectVolume, volume);
            audioMixer.SetFloat("volume_effects", volume);
        }

        public void LeftHanded(bool lH)
        {
            if (StaticLevel.noHands) return;
            if (StaticLevel.noStick) return;

            //if true enable right hand visuals, disable left
            if (lH)
            {
                StaticLevel.leftHand = true;
                playerSettings.DisableHands(0);
                playerSettings.EnableHands(1);
            }
            else
            {
                StaticLevel.leftHand = false;
                playerSettings.DisableHands(1);
                playerSettings.EnableHands(0);
            }
            PlayerPrefs.SetInt(leftHandString, (lH) ? 1 : 0);
        }

        public void NoHands(bool nH)
        {
            if (StaticLevel.noStick) return;
            //make pickup and interact colliders bigger, make pickup happen on collision, just select different prefabs on collection. Don't require taking objects to storage.
            //Can use gaze and space to select menu options, or use joystick and space.
            //elevator buttons if part of the objective and next level will automatically press when wheelchair is close
            // only allow as a main menu option
            StaticLevel.noHands = nH;
            PlayerPrefs.SetInt(noHandsString, (nH) ? 1 : 0);
            if (nH)
            {
                playerSettings.DisableHands(0);
                playerSettings.DisableHands(1);
            }
            else
            {
                LeftHanded(StaticLevel.leftHand);
            }
            if (allowNoHandsToggle == false) return;
            SetActivation();
        }

        public void EnableTunnelTurn(bool activate)
        {
            StaticLevel.turningTunnelVision = activate;
            PlayerPrefs.SetInt(turnTunnelingString, (activate) ? 1 : 0);
        }

        public void EnableTunnelMove(bool activate)
        {
            StaticLevel.accelerationTunnelVision = activate;
            PlayerPrefs.SetInt(moveTunnelingString, (activate) ? 1 : 0);
        }

        public void SetTunnelAmount(float amount)
        {
            StaticLevel.tunnelVisionAmount = amount;
            PlayerPrefs.SetFloat(amountTunnelingString, amount);
        }

        public void SetTurnStep(float amount)
        {
            StaticLevel.turnStepAmount = Mathf.FloorToInt(amount);
           // StaticLevel.smoothMovement = StaticLevel.turnStepAmount == 0 && StaticLevel.movementStepAmount == 0 && StaticLevel.acceleration;
            PlayerPrefs.SetInt(turnStepString, Mathf.FloorToInt(amount));
        }
        public void SetMoveStep(float amount)
        {
            StaticLevel.movementStepAmount = Mathf.FloorToInt(amount);
         //   StaticLevel.smoothMovement = StaticLevel.turnStepAmount == 0 && StaticLevel.movementStepAmount == 0 && StaticLevel.acceleration;
            PlayerPrefs.SetInt(moveStepString, Mathf.FloorToInt(amount));
        }
        public void EnableAcceleration(bool active)
        {
            StaticLevel.acceleration = active;
            PlayerPrefs.SetInt(accelerationString, (active) ? 1 : 0);
        //    StaticLevel.smoothMovement = StaticLevel.turnStepAmount == 0 && StaticLevel.movementStepAmount == 0 && StaticLevel.acceleration;
        }

        public void NoWheelchairStick(bool nW)
        {
            if (StaticLevel.noHands) {
                
                return;
            }
            StaticLevel.noStick = nW;
            PlayerPrefs.SetInt(noStickString, (nW) ? 1 : 0);
            //enable both hands
            if (nW)
            {
                playerSettings.EnableHands(0);
                playerSettings.EnableHands(1);
            }
            else
            {
                LeftHanded(StaticLevel.leftHand);
            }
            SetActivation();
        }

        public void ObjectiveMarkers(bool allow)
        {
            //Prevent / allow creation of more quest markers
            StaticLevel.enableQuestMarkers = allow;
            QuestSystem q = QuestSystem.instance;
            if (q != null)
            {
                q.QuestMarkers(!allow);
            }
            PlayerPrefs.SetInt(enableQuestMarkersString, (allow) ? 1 : 0);
        }

        public void Speed(float speed)
        {
            StaticLevel.maxSpeed = speed;
            PlayerPrefs.SetFloat(maximumSpeed, speed);
        }

        public void EnableSmoothMovement(bool active)
        {
            StaticLevel.smoothMovement = active;
            PlayerPrefs.SetInt(enableSmoothMovementString, (active) ? 1 : 0);
        }

        #endregion
    }
}