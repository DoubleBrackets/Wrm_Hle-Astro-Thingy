using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public static class ExtensionMethods 
{
    public static Vector2 GetMouseVector2D(this Vector3 vector3,Vector3 pos)
    {
        Vector2 diff = CameraScript.main.ScreenToWorldPoint(vector3) - pos;
        return diff;
    }
    /// <summary>
    /// Takes in a Vector2 and returns the angle in degrees
    /// </summary>
    public static float Angle(this Vector2 Vector2)
    {
        float diff = Mathf.Atan2(Vector2.y, Vector2.x) * Mathf.Rad2Deg;
        return diff;
    }

    /// <summary>
    /// Takes in an angle in degrees and returns the unit vector
    /// </summary>
    public static Vector2 UnitVector(this float ang)
    {
        ang *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
    }
    /// <summary>
    /// Checks for an NaN values
    /// </summary>
    public static bool IsNan(this Vector2 v)
    {
        if (float.IsNaN(v.x * v.y))
            return true;
        return false;
    }
    public static void SetZAngle(this Transform transform,float val)
    {
        transform.rotation = Quaternion.Euler(0, 0, val);
    }
    public static Vector2 PixelClamp(this Vector2 pos,int pixelsPerUnit)
    {
        Vector2 newPos = new Vector2(Mathf.RoundToInt(pos.x * pixelsPerUnit), Mathf.RoundToInt(pos.y * pixelsPerUnit));

        return newPos / pixelsPerUnit;
    }

    public static Vector2 GetIntersectionPoint(Vector2 pos1a,Vector2 pos1b, Vector2 pos2a, Vector2 pos2b)
    {
        Vector2 vector1 = pos1b - pos1a;
        Vector2 vector2 = pos2b - pos2a;

        float m1 = vector1.y / vector1.x;
        float m2 = vector2.y / vector2.x;

        if (m1 == m2)
            return Vector2.zero;

        float xIntersection = (m1 * pos1a.x - m2 * pos2a.x + pos2a.y - pos1a.y) / (m1 - m2);

        if(xIntersection.IsBetween(pos1a.x,pos1b.x) && xIntersection.IsBetween(pos2a.x,pos2b.x))
        {
            Vector2 final = new Vector2(xIntersection, m1 * (xIntersection - pos1a.x) + pos1a.y);
            if (final == Vector2.zero)
                final += Vector2.up * 0.01f;
            return final;
        }

        return Vector2.zero;
    }

    public static Vector2 PerpendicularVector(this Vector2 vec,int dir)
    {
        if (dir == 1)
            return new Vector2(-vec.y, vec.x);
        else if (dir == 0)
            return vec;
        return new Vector2(vec.y, -vec.x);
    }

    public static bool IsBetween(this float val, float a, float b)
    {
        float lower = Mathf.Min(a, b);
        float upper = Mathf.Max(a, b);
        if(val >= lower && val <= upper)
        {
            return true;
        }
        return false;
    }


    public static Coroutine StopCoroutineCheckNull(this Coroutine coroutine, MonoBehaviour mono)
    {
        if(coroutine != null)
        {
            mono.StopCoroutine(coroutine);
            coroutine = null;
        }
        return coroutine;
    }
}
