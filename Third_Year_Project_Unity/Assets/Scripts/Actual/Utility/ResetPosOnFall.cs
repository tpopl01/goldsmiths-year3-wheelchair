using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Util
{
    //While there is not collision on the users hands or spring like physics
    //the user can put their arm through walls and drop an object
    //thus making the level impossible to complete.
    //This script attempts to find a solution to that until more rubust physics are developed
    //This script will simply reset the objects postion to its starting point if it falls below 0 in the y axis.
    public class ResetPosOnFall : MonoBehaviour
    {
        Vector3 pos;

        #region initialise position
        void Start()
        {
            pos = transform.position;
        }

        void OnEnable()
        {
            pos = transform.position;
        }
        #endregion

        void Update()
        {
            if (transform.position.y < 0 && pos.y >= 0)
            {
                transform.position = pos;
            }
        }
    }
}