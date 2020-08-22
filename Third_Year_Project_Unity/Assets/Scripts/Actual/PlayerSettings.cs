using System.Collections;
using System.Collections.Generic;
using tpopl001.Controller;
using tpopl001.Questing;
using UnityEngine;

[CreateAssetMenu(menuName ="Player/Settings")]
//Simple Script to hold objects relevant to the player.
//Other Scripts can then reference this script
//This saves memory by only referencing one point in memory 
//and saves expense from preventing the need to call 'FindObjectOfType' from every script that wants to reference these components
public class PlayerSettings : ScriptableObject
{
    private Transform hmd;
    //get the players head transform. Set the variable if it is null.
    public Transform HMD
    {
        get
        {
            if (hmd == null)
                hmd = GameObject.FindObjectOfType<OVRCameraRig>().transform;
            return hmd;
        }
    }

    private OVRGrabber[] hands;
    //get the players hand transforms. Set the variable if it is null.
    public OVRGrabber[] Hands
    {
        get
        {
            if (hands == null)
                hands = GameObject.FindObjectsOfType<OVRGrabber>();
            if (hands.Length==0)
                hands = GameObject.FindObjectsOfType<OVRGrabber>();
            if (hands[0] == null)
                hands = GameObject.FindObjectsOfType<OVRGrabber>();
            return hands;
        }
    }

    private GameObject wheelchair;
    //get the wheelchair transform. Set the variable if it is null.
    public GameObject Wheelchair
    {
        get
        {
            if(wheelchair==null)
            {
                wheelchair = GameObject.FindObjectOfType<PoweredWheelchairRBController>().gameObject;
                if (!StaticLevel.noHands)
                {
                    wheelchair.GetComponentInChildren<CollectionTrigger>(true).gameObject.SetActive(false);
                }
            }
            return wheelchair;
        }
    }

    //activate hand
    public void DisableHands(int index)
    {
        index = Mathf.Clamp(index, 0, Hands.Length);
        Hands[index].gameObject.SetActive(false);
    }

    //de-activate hand
    public void EnableHands(int index)
    {
        index = Mathf.Clamp(index, 0, Hands.Length);
        Hands[index].gameObject.SetActive(true);
    }

}
