using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class LogManager
{
    private static bool enableLog;

    public static void SetEnableLog(bool _enableLog)
    {
        enableLog = _enableLog;
    }

    public static void LogMessage(Object sender, string message)
    {
        if (!enableLog) return;
        Debug.Log($"{sender.name} - {message}");
    }
}
