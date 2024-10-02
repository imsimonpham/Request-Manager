using System;
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.Android;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private AndroidNotifications _androidNotifications;

    private void Start()
    {
        _androidNotifications.RequestAuthorization();
        _androidNotifications.RegisterNotificationChannel();
    }

    /*private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            _androidNotifications.SendNotification("New Request", "You have a new request", 3);
        }
    }*/
}
