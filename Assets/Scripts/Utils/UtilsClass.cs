using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsClass : MonoBehaviour
{
    public static float GetAngleFromVector(Vector3 dir){
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(n < 0){
            n += 360;
        }
        return n;
    }

}
