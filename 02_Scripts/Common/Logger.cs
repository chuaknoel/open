using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class Logger
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(object logMsg)
    {
        Debug.Log($"<color= #ffff00>{logMsg}</color>");
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogError(object logMsg)
    {
        Debug.LogError($"<color= #ffff00>{logMsg}</color>");
    }

    [Conditional("UNITY_EDITOR")]
    public static void DrawLine(Ray ray, RaycastHit hit , float duration = 2f)
    {
        Debug.DrawLine(ray.origin, hit.point, Color.red,duration);
    }
}
