using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.Android;

public class AndroidNotifications : MonoBehaviour
{
    [SerializeField] private List<string> _requestIds;
    
    //request authorization to send notis
    public void RequestAuthorization()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATION"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATION");
        }
    }
    
    //register a noti channel
    public void RegisterNotificationChannel()
    {
        var channel = new AndroidNotificationChannel
        {
            Id = "default_channel",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "New Request"
        };
        
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
    
    //set up noti template
    public void SendNotification(string title, string text, int fireTime)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.SmallIcon = "icon_0";
        notification.ShowTimestamp = true;
        notification.FireTime = System.DateTime.Now.AddSeconds(fireTime);

        AndroidNotificationCenter.SendNotification(notification, "default_channel");
        Debug.Log("sent");
    }
    
    /*if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
       {
           AndroidNotificationCenter.CancelAllNotifications();
           notification.FireTime = System.DateTime.Now.AddSeconds(fireTime);
       }*/
}
