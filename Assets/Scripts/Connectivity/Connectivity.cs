using UnityEngine;

public class Connectivity : MonoBehaviour
{
    public bool IsConnectedToInternet()
    {
        return (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ||
                Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork);
    }
}
