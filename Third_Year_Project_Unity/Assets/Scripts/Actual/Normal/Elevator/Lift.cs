using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private LiftObject lift = null;
    [SerializeField] private LiftFloor[] liftFloors = null;
    [SerializeField] private float speed = 0.3f;
    [SerializeField] private bool openDoors = true;

    private int currentLevel = 0;
    private int offset = 0;
    private bool moving = false;
    private bool open;
    private bool close;

    void Update()
    {
        UpdateMovement();
    }

    /// <summary>
    /// Handles the closing and opening of doors and the movement of the lift
    /// </summary>
    void UpdateMovement()
    {
        if (open)
        {
            open = !liftFloors[currentLevel].OpenDoors(0.5f) && !lift.OpenDoors(0.8f);
        }
        else if (close)
        {
            close = !liftFloors[currentLevel + offset].CloseDoors(0.5f) || !lift.CloseDoors(0.5f) || !liftFloors[currentLevel].CloseDoors(0.5f); //changed && to ||
            if (!close) offset = 0;
        }
        else if (moving)
        {
            moving = lift.MoveToPos(liftFloors[currentLevel].GetLiftPos(), speed);
            if (moving == false && openDoors) open = true;
        }
    }

    #region Functions OnEvent
    /// <summary>
    /// Close doors, then move lift to specified floor
    /// </summary>
    public void SetAndMoveToTagetLevel(int level)
    {
        if (lift.GetInTransit() || open || close)
            return;
        moving = false;

        if (!lift.CloseDoors(10))
        {
            offset = currentLevel - Mathf.Clamp(level, 0, liftFloors.Length - 1);
            Close();
        }
        currentLevel = Mathf.Clamp(level, 0, liftFloors.Length - 1);
        moving = true;
    }

    /// <summary>
    /// Close doors and move down a floor
    /// </summary>
    public void Down()
    {
        if (currentLevel <= 0)
            return;

        if (lift.GetInTransit() || open || close)
            return;

        //close current floor
        if(!lift.CloseDoors(10))
        {
            offset = 1;
            Close();
        }

        currentLevel--;
        moving = true;
    }

    /// <summary>
    /// Close doors and move up a floor
    /// </summary>
    public void Up()
    {
        if (currentLevel >= liftFloors.Length - 1)
            return;

        if (lift.GetInTransit() || open || close)
            return;

        if (!lift.CloseDoors(10))
        {
            Close();
            offset = -1;
        }

        currentLevel++;
        moving = true;
    }

    /// <summary>
    /// Open doors if not already performing an action
    /// </summary>
    public void Open()
    {
        if (lift.GetInTransit() || open || close || moving)
            return;
        open = true;
    }

    /// <summary>
    /// Close doors if not already performing an action
    /// </summary>
    public void Close()
    {
        if (lift.GetInTransit()|| open || close || moving)
            return;
        close = true;
    }
    #endregion
}

[System.Serializable]
public class LiftFloor
{
    [SerializeField] private Door[] doors = null;
    [SerializeField] private Vector3 localPos = Vector3.zero;

    public Vector3 GetLiftPos()
    {
        return localPos;
    }

    /// <summary>
    /// Opens doors, returns true once both doors are open
    /// </summary>
    public bool OpenDoors(float speed)
    {
        bool doorsOpen = true;
        float delta = Time.deltaTime;
        for (int i = 0; i < doors.Length; i++)
        {
            if (!doors[i].OpenDoor(speed, delta))
            {
                doorsOpen = false;
            }
        }
        return doorsOpen;
    }

    /// <summary>
    /// Closes doors, returns true once both doors are closed
    /// </summary>
    public bool CloseDoors(float speed)
    {
        bool doorsClosed = true;
        float delta = Time.deltaTime;
        for (int i = 0; i < doors.Length; i++)
        {
            if (!doors[i].CloseDoor(speed, delta))
            {
                doorsClosed = false;
            }
        }
        return doorsClosed;
    }
}

[System.Serializable]
public class Door
{
    [SerializeField] private Vector3 startPos = Vector3.zero;
    [SerializeField] private Vector3 endPos = Vector3.zero;
    [SerializeField] private Transform door = null;

    /// <summary>
    /// Closes doors by interpolating them to the start position, returns true once both doors are closed
    /// </summary>
    public bool CloseDoor(float speed, float delta)
    {
        door.localPosition = Vector3.Lerp(door.localPosition, startPos, speed * delta);
        return Vector3.Distance(door.localPosition, startPos) < 0.001f;
    }

    /// <summary>
    /// Opens doors by interpolation them to the end position, returns true once both doors are open
    /// </summary>
    public bool OpenDoor(float speed, float delta)
    {
        door.localPosition = Vector3.Lerp(door.localPosition, endPos, speed * delta);
        return Vector3.Distance(door.localPosition, endPos) < 0.001f;
    }
}

[System.Serializable]
public class LiftObject
{
    [SerializeField]private Transform lift = null;
    [SerializeField]private Door[] doors = null;
    private bool inTransit;

    public bool GetInTransit()
    {
        return inTransit;
    }

    /// <summary>
    /// Interpolates the lift to the target position
    /// </summary>
    public bool MoveToPos(Vector3 pos, float speed)
    {
        pos.x = lift.localPosition.x;
        pos.z = lift.localPosition.z;
        lift.localPosition = Vector3.Lerp(lift.localPosition, pos, speed * Time.deltaTime);
        inTransit = Vector3.Distance(lift.localPosition, pos) > 0.005f;
        return inTransit;
    }

    /// <summary>
    /// Opens doors, returns true once both doors are open
    /// </summary>
    public bool OpenDoors(float speed)
    {
        bool doorsOpen = true;
        float delta = Time.deltaTime;
        for (int i = 0; i < doors.Length; i++)
        {
            if(!doors[i].OpenDoor(speed, delta))
            {
                doorsOpen = false;
            }
        }
        return doorsOpen;
    }

    /// <summary>
    /// Closes doors, returns true once both doors are closed
    /// </summary>
    public bool CloseDoors(float speed)
    {
        bool doorsClosed = true;
        float delta = Time.deltaTime;
        for (int i = 0; i < doors.Length; i++)
        {
            if (!doors[i].CloseDoor(speed, delta))
            {
                doorsClosed = false;
            }
        }
        return doorsClosed;
    }
}