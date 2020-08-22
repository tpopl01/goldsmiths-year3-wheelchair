using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Util
{
    //simple script that rotates the gameObject to look at the target
    public class LookAtTransform : MonoBehaviour
    {
        [SerializeField] private Transform lookAtObj;

        private void Start()
        {
            if (lookAtObj == null)
                lookAtObj = GameObject.FindObjectOfType<OVRCameraRig>().trackerAnchor;
            Debug.Log(lookAtObj);
        }

        private void Update()
        {
            transform.LookAt(lookAtObj);
        }
    }
}
