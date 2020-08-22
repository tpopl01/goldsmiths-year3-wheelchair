using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Triggers
{
    public class HandleTurn : OpenDoor
    {
        [SerializeField] private float pullHandleDist = 0.1f;
        private Rigidbody handleRB;

        protected override void Init()
        {
            base.Init();
            handleRB = GetComponentInParent<Rigidbody>();
            doorRB = handleRB.GetComponentInParent<Rigidbody>();
        }

        protected override void OnInteracting(OVRGrabber grabber)
        {
            if (grabber.transform.position.y + pullHandleDist < transform.position.y)
            {
                base.OnInteracting(grabber);
            }
        }

        protected override void FixedInteracting()
        {
            base.FixedInteracting();
            handleRB.MoveRotation(transform.rotation);
        }
    }
}