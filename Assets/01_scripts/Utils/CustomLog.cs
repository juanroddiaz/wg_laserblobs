using UnityEngine;

public class CustomLog : MonoBehaviour
{
    public static void Log(string logToPrint)
    {
#if !RELEASE
        Debug.Log(logToPrint);
#endif
    }

    public static void LogWarning(string logToPrint)
    {
#if !RELEASE
        Debug.LogWarning(logToPrint);
#endif
    }

    public static void LogError(string logToPrint)
    {
#if !RELEASE
        Debug.LogError(logToPrint);
#endif
    }
}
