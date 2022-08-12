using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static bool CheckDistance(Vector3 first,Vector3 second,float distance)
    {
        return Vector3.SqrMagnitude(second - first) <= distance * distance;
    }
}
