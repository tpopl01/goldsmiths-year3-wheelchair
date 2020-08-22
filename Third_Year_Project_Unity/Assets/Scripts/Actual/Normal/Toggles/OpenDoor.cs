using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Triggers
{
    public class OpenDoor : ToggleBase
    {
        [SerializeField]protected Rigidbody doorRB;
        private bool isInteracting;

        #region Initialisation
        void Start()
        {
            Init();
        }

        //remove the door if no hands,
        //Otherwise initialise the variables
        protected virtual void Init()
        {
            if(StaticLevel.noHands)
            {
                Disable();
                return;
            }

            if (doorRB == null)
            doorRB = GetComponentInParent<Rigidbody>();
        }
        #endregion


        protected override void NotInteracting()
        {
            if (isInteracting)
            {
                //reset the handles grab point
                isInteracting = false;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(Vector3.zero);
                //reset the velocity so the door doesn't swing out of control
                doorRB.velocity = Vector3.zero;
                doorRB.angularVelocity = Vector3.zero;
            }
        }

        protected override void OnInteracting(OVRGrabber grabber)
        {
            //follow the users hand position
            transform.position = grabber.transform.position;
            transform.rotation = grabber.transform.rotation;
            isInteracting = true;
        }

        void FixedUpdate()
        {
            if (isInteracting)
            {
                FixedInteracting();
            }
        }

        //smoothly lerp the door model to this objects position
        protected virtual void FixedInteracting()
        {
            Vector3 targetPos = transform.position;
            targetPos.y = doorRB.position.y;
            targetPos = Vector3.Lerp(doorRB.position, targetPos, Time.deltaTime * 10);
            doorRB.MovePosition(targetPos);
        }

        void Disable()
        {
            transform.parent.parent.gameObject.SetActive(false);
        }

    }
}