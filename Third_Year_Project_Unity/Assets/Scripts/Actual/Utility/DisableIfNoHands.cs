using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Util
{
    //A simple utility script to disable the object if the user has enabled the no hand mode
    //This will remove irrelevant text messages or hints from the game
    public class DisableIfNoHands : MonoBehaviour
    {
        [SerializeField] bool noHands = true;
        void Start()
        {
            if (StaticLevel.noHands == noHands)
            {
                gameObject.SetActive(false);
            }
        }
    }
}