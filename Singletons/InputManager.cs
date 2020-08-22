using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton class dedicated to providing a single location in which I can edit key presses etc.
public class InputManager : MonoBehaviour
{
    /// <summary>
    /// Get keyboard / VR controller axis,
    /// Otherwise get VR controller axis
    /// </summary>
    public Vector2 GetAxisMove()
    {
        Vector2 axis = Vector2.zero;
        axis.x = Input.GetAxis("Horizontal");
        axis.y = Input.GetAxis("Vertical");
        if (axis == Vector2.zero) axis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        if (axis == Vector2.zero) axis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        return axis;
    }

    /// <summary>
    /// Are we pressing the pause button
    /// </summary>
    public bool PauseButton()
    {
        return OVRInput.GetDown(OVRInput.Button.Four) || OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.Escape);
    }

    void Update()
    {
        Recalibrate();
    }

    /// <summary>
    /// Button to reset the player position and rotation to align properly with the chair
    /// </summary>
    public void Recalibrate()
    {
        if(Input.GetKeyDown(KeyCode.R) || OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
        {
            //UnityEngine.XR.InputTracking.Recenter();
           OVRManager.display.RecenterPose();
        }
    }

    /// <summary>
    /// Button to open objectives panel
    /// </summary>
    public bool OpenJournelButton()
    {
        return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetKeyDown(KeyCode.J);
    }


    #region Setup
    public static InputManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
}
