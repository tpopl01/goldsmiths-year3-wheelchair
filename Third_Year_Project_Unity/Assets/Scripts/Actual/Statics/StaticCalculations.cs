using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that hosts any calculation that are likely to be used multiple times
public static class StaticCalculations
{
  

    public static float GetAngle(Vector3 targetPos, Vector3 currentPos, Vector3 currentForward)
    {
        Vector3 dir = GetDirection(targetPos, currentPos);
        return Vector3.Angle(currentForward, dir);
    }

    public static Vector3 GetDirection(Vector3 targetPos, Vector3 currentPos)
    {
        Vector3 directionToLookTo = targetPos - currentPos;
        directionToLookTo.y = 0;

        return directionToLookTo;
    }

    public static Quaternion GetLookRotation(Vector3 targetPos, Vector3 currentPos, Vector3 currentForward, out bool shouldRotate, float threshold = 0.1f)
    {
        shouldRotate = false;
        Vector3 dir = GetDirection(targetPos, currentPos);
        float angle = Vector3.Angle(currentForward, dir);
        if (angle > threshold)
        {
            shouldRotate = true;
            return Quaternion.LookRotation(dir);
        }

        return Quaternion.identity;
    }

    public static string ProcessObjectName(string name)
    {
        if (name.Length > 7)
            name = name.Remove(name.Length - 7);
        return name;
    }
}
