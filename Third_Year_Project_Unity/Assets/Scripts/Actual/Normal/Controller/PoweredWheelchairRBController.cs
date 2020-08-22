using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Controller
{
    public class PoweredWheelchairRBController : MonoBehaviour
    {
        private Rigidbody rb;
        [SerializeField] private float acceleration = 20;
        private float turnSpeed = 0.005f;
        private float speed;
        private bool onGround = false;

        [Space]
        [Header("Wheelchair Settings")]
        [SerializeField] private RigidbodyWheel[] wheels = null;
        [SerializeField] private float minAngularDrag = 10;
        [SerializeField] private float maxAngularDrag = 30;
        [SerializeField] private float drag = 5;
        [SerializeField][Range(0.1f, 2)] private float speedMulti = 1;
        private Transform player;

        [SerializeField] bool playerUpdateSmooth = false;

        public Transform model;

        [SerializeField][Range(0,10)] float lerpSpeed = 3f;
        [SerializeField] [Range(0, 10)] float turnSmooth = 4f;
        [SerializeField] AudioSource aS = null;
        Vector3 tP;

        private int tick;
        public int tickUpdate = 0;
        private int turnTick = 0;

        enum Surface
        {
            None,
            Water
        }
        Surface currentSurface = Surface.None;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            player = FindObjectOfType<OVRCameraRig>().transform;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
            aS.loop = true;
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
        }

        void Update()
        {
            tP = rb.position;
            HandleDrag();
            GroundAngle(false);
            if(StaticLevel.movementStepAmount == 0)
                HandlePlayerPos();
            if (StaticLevel.turnStepAmount == 0)
            {
                HandlePlayerRot();
            }else
                model.transform.rotation = Quaternion.Slerp(model.transform.rotation, transform.rotation, Time.deltaTime * turnSmooth);
        }

        void HandleDrag()
        {
            //if the chair is not on the ground then remove drag from the rigid body and enable gravity
            if (!onGround)
            {
                rb.drag = 0;
                rb.useGravity = true;
            }
            else
            {
                //set the drag depending on the surface
                if (currentSurface == Surface.Water)
                    rb.drag = drag / 2;
                else
                    rb.drag = drag;
                rb.useGravity = false;
            }
        }

        void FixedUpdate()
        {
            Vector2 moveAxis = InputManager.instance.GetAxisMove();

            GroundAngle(true);
            HandleMovement(moveAxis);
            FixedMoveStep();
            FixedTurnStep();
        }

        #region Player Movement
        #region Step Movement
        void FixedMoveStep()
        {
            if (StaticLevel.movementStepAmount != 0)
            {
                tick++;
                if (tick >= StaticLevel.movementStepAmount*10)
                {
                    tick = 0;
                    player.position = transform.position;
                    model.transform.position = transform.position;
                }
            }
        }

        void FixedTurnStep()
        {
            if (StaticLevel.turnStepAmount > 0)
            {
                turnTick++;
                if (turnTick >= StaticLevel.turnStepAmount * 10)
                {
                    turnTick = 0;
                    Vector3 pos = transform.position + transform.forward * 100;
                    pos -= player.position;
                    pos.y = 0;
                    Quaternion targetRot = Quaternion.LookRotation(pos);
                    player.rotation = targetRot;
                }
            }
        }
        #endregion

        #region Normal Movement
        void HandlePlayerPos()
        {
            if (StaticLevel.smoothMovement)
            {
                player.position = Vector3.Lerp(player.position, tP, Time.deltaTime * lerpSpeed);
                model.transform.position = Vector3.Lerp(model.transform.position, tP, Time.deltaTime * lerpSpeed);
            }
            else
            {
                player.position = tP;
                model.transform.position = tP;
            }
        }
        
        void HandlePlayerRot()
        {
            if (StaticLevel.smoothMovement)
            {
                SetRot(transform.position + transform.forward * 100, player.forward, player, true);
                SetRot(transform.position + transform.forward * 100, model.forward, model, false);
            }
            else
            {
                //rotate player to face wheelchair forward, lock local x axis
                Vector3 pos = (transform.position + transform.forward * 100) - player.position;
                pos.y = 0;
                Quaternion targetRot = Quaternion.LookRotation(pos);
                player.rotation = targetRot;
                //rotate wheelchair model
                targetRot = Quaternion.LookRotation((transform.position + transform.forward * 100) - model.position);
                model.rotation = targetRot;
            }
        }

        void SetRot(Vector3 targetPos, Vector3 forward, Transform actualPos, bool yZero)
        {
            Vector3 pos = targetPos - actualPos.position;
            if (yZero) pos.y = 0;
            float angle = Vector3.Angle(forward, pos);
            if (angle > 0.01f)
            {
                Quaternion tarR = Quaternion.LookRotation(pos);
                actualPos.rotation = Quaternion.Slerp(actualPos.rotation, tarR, Time.deltaTime * turnSmooth);
            }
        }

        #endregion
        #endregion


        #region Wheelchair Movement
        //adds force and torque to the wheelchair
        void HandleMovement(Vector2 moveAxis)
        {
            float delta = Time.fixedDeltaTime;

            if (moveAxis.y == 0)
            {
                IdleTurn(moveAxis, delta);
            }
            else
            {
                speed = GetSpeed(moveAxis, delta);
                ClampSpeed();
                float speedMultiplier = 0.8f;
                float sM = Mathf.Abs(transform.rotation.x / 10);
                float buffedSpeed = speed * ((transform.rotation.x < 0f) ? 1 - sM : 1 + sM) * speedMulti;
                buffedSpeed = Mathf.Clamp(buffedSpeed * 0.05f * StaticLevel.maxSpeed, -(StaticLevel.maxSpeed - 2), StaticLevel.maxSpeed + ((transform.rotation.x < 0f) ? 1 - sM : 1 + sM)) * speedMultiplier;
                if (StaticLevel.acceleration == false) buffedSpeed = speed * speedMultiplier * 0.005f * StaticLevel.maxSpeed;

                Vector3 force = transform.forward * (buffedSpeed) / delta;
                force.y = 0;
                rb.AddForce(force, ForceMode.Force);
                rb.AddTorque((transform.up * moveAxis.x * turnSpeed) / delta);

                PlayAudio(moveAxis, buffedSpeed / delta);
                UpdateVisuals(buffedSpeed, moveAxis.x * turnSpeed, moveAxis.x);
            }
        }

        //angular drag is reset to the minimum value
        // torque is applied based on horizontal axis input
        void IdleTurn(Vector2 moveAxis, float delta)
        {
            speed = Mathf.Lerp(speed, 0, delta * 5);
            ClampSpeed();
            if(rb.angularDrag != minAngularDrag)
                rb.angularDrag = minAngularDrag;
            rb.AddTorque((transform.up * moveAxis.x * turnSpeed) / delta);
            PlayAudio(moveAxis, (moveAxis.x * turnSpeed) / delta);
        }

        //method determines and returns the speed. It increases angular drag the faster the chair is moving
        float GetSpeed(Vector2 axis, float delta)
        {
            if(StaticLevel.acceleration)
            {
                if (axis.y > 0)
                {
                    if (speed < 0)
                    {
                        speed += 5 * delta / acceleration;
                        rb.angularDrag -= delta;
                    }
                    else
                    {
                        if (speed < 1)
                        {
                            rb.angularDrag += delta;
                        }
                        speed += delta / acceleration;
                    }
                }
                else if (axis.y < 0)
                {
                    if (speed > 0)
                    {
                        speed -= 5 * delta / acceleration;
                        rb.angularDrag -= delta;
                    }
                    else
                    {
                        speed -= delta / acceleration;
                        rb.angularDrag += delta;
                    }
                }
                return speed;
            }
            rb.angularDrag = Mathf.Clamp(maxAngularDrag * axis.y, minAngularDrag, maxAngularDrag);
            return axis.y;
        }

        //rotates the wheel models based on the speed
        void UpdateVisuals(float speed, float turnAmount, float h)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                float a = speed;
                if (a == 0 && h != 0) a = h;
                wheels[i].GetWheel().transform.RotateAround(wheels[i].GetWheel().transform.position, wheels[i].GetWheel().transform.right, Time.deltaTime * a * 3000);
                if (wheels[i].GetSteering())
                {
                    if (turnAmount == 0 && speed != 0)
                    {
                        wheels[i].GetWheel().transform.parent.localRotation = Quaternion.Slerp(wheels[i].GetWheel().transform.parent.localRotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * 3f);
                    }
                    else
                        wheels[i].GetWheel().transform.parent.RotateAround(wheels[i].GetWheel().transform.parent.position, wheels[i].GetWheel().transform.parent.up, Time.deltaTime * turnAmount * (speed == 0 ? -10000 : (speed < 0) ? 7000 : -7000));

                    float y = wheels[i].GetWheel().transform.parent.localEulerAngles.y;
                    float rotAmount = 95 / Mathf.Abs(rb.velocity.magnitude);
                    rotAmount = Mathf.Clamp(rotAmount, 0, 90);
                    if (y > rotAmount && y < 360 - rotAmount - 10)
                    {
                        y = rotAmount;
                    }
                    else if (y < 360 - rotAmount && y > rotAmount + 10)
                    {
                        y = 360 - rotAmount;
                    }
                    wheels[i].GetWheel().transform.parent.localRotation = Quaternion.Euler(wheels[i].GetWheel().transform.parent.localEulerAngles.x, y, wheels[i].GetWheel().transform.parent.localEulerAngles.z);
                }
            }
        }

        #endregion
        // play an engine sound when moving
        void PlayAudio(Vector2 moveAxis, float volume)
        {
            volume = Mathf.Clamp(Mathf.Abs(volume), 0.0f, 0.3f);
            aS.volume = volume;
            if (moveAxis.x != 0 || moveAxis.y != 0)
            {
                if(!aS.isPlaying)
                    aS.Play();
            }
            else if (aS.isPlaying)
            {
                aS.Pause();
            }
        }

        //determine whether or not the wheelchair is on the ground.
        void GroundAngle(bool fixedUp)
        {
            RaycastHit bHit;
            onGround = false;
            if (Physics.Raycast(transform.position + (transform.up * 0.5f), -transform.up, out bHit, 2f, ~(1 << 11), QueryTriggerInteraction.Ignore))
            {
                onGround = true;
                SetSurface(bHit.transform);

                if (fixedUp)
                {
                    SetChairToGround(bHit);
                    AlignToGroundAngle(bHit);
                }
            }
            else
            {
                onGround = Physics.Raycast(transform.position + (transform.up * 0.5f) - transform.forward * 0.5f, -transform.up, 2f, ~(1 << 11), QueryTriggerInteraction.Ignore) ||
                    Physics.Raycast(transform.position + (transform.up * 0.5f) + transform.forward * 0.5f, -transform.up, 2f, ~(1 << 11), QueryTriggerInteraction.Ignore);
            }
        }

        void SetChairToGround(RaycastHit hit)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.y = hit.point.y;
            if (playerUpdateSmooth)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 20);
            }
            else transform.position = (targetPosition);
        }

        //determine the surface type
        void SetSurface(Transform floor)
        {
            if (floor.CompareTag("water"))
                currentSurface = Surface.Water;
            else
                currentSurface = Surface.None;
        }

        //fire two down and align the chair to them. Clamp the chairs max rotation to prevent climbing very steep walls
        //If I wanted rotation along the z axis aswell as the x axis I would fire one ray down and align the wheelchair to the rays normal
        void AlignToGroundAngle(RaycastHit hit)
        {
            if (Physics.Raycast(transform.position + (transform.up * 0.5f) - transform.forward * 0.5f, -transform.up, out hit, 2f, ~(1 << 11), QueryTriggerInteraction.Ignore))
            {
                if (Physics.Raycast(transform.position + (transform.up * 0.5f) + transform.forward * 0.5f, -transform.up, out RaycastHit fHit, 2f, ~(1 << 11), QueryTriggerInteraction.Ignore))
                {
                    Vector3 angle = fHit.point - hit.point;
                    Vector3 targetRot = angle;
                    transform.forward = Vector3.Slerp(transform.forward, targetRot, Time.fixedDeltaTime * turnSmooth);
                    targetRot = transform.eulerAngles;
                    float clampAngle = 50;
                    if (targetRot.x > clampAngle && targetRot.x < 360 - clampAngle)
                    {
                        if (Mathf.Abs(360 - clampAngle - targetRot.x) < Mathf.Abs(clampAngle - targetRot.x))
                        {
                            targetRot.x = 360 - clampAngle;
                        }
                        else
                        {
                            targetRot.x = clampAngle;
                        }
                    }
                    transform.rotation = Quaternion.Euler(targetRot);
                }
            }
        }

        void ClampSpeed()
        {
            speed = Mathf.Clamp(speed, -0.5f,1);
            rb.angularDrag = Mathf.Clamp(rb.angularDrag, minAngularDrag, maxAngularDrag);
        }
    }

    [System.Serializable]
    public class RigidbodyWheel
    {
        [SerializeField] GameObject wheel;
        [SerializeField] bool steering = false;

        public GameObject GetWheel()
        {
            return wheel;
        }

        public bool GetSteering()
        {
            return steering;
        }

    }

}