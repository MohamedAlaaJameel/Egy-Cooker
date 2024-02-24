using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;


public static class ExtensionMethods
{
    public static void RemoveAllListenersSafe(this Button button)
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            int persistentCount = button.onClick.GetPersistentEventCount();
            for (int i = 0; i < persistentCount; i++)
            {
                UnityEventTools.RemovePersistentListener(button.onClick, 0);
            }
        }
        else
        {
            Debug.LogError("Button is Null");
        }
    }
}


