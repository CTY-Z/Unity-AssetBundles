using System.Diagnostics;

public static class Common
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void Warning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void Error(object message)
    {
        UnityEngine.Debug.LogError(message);
    }
}
