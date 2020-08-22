using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StaticLevel
{
    #region Level
    public static string username;
    public static string levelName;
    public static string levelTitle;
    public static string objectiveTitle;
    #endregion

    #region Settings
    public static float maxSpeed = 5;
    public static bool enableQuestMarkers = true;

    #region Hands
    public static bool noHands = true;
    public static bool noStick = true;
    public static bool leftHand = false;
    #endregion

    #region Comfort
    public static bool acceleration = true;
    public static bool smoothMovement = false;
    public static float tunnelVisionAmount = 0.75f;
    public static bool accelerationTunnelVision = false;
    public static bool turningTunnelVision = true;
    public static int movementStepAmount = 0;
    public static int turnStepAmount = 0;
    #endregion

    #endregion
}
