using UnityEngine;
using UnityEngine.UIElements;

public class PendingRequestsTab : MonoBehaviour
{
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private UIUtilities _uiUtilities;
    [SerializeField] private RequestCard _requestCard; //debug
    private VisualElement _pendingRequestsTab;
    private ScrollView _cardContainer;
    private VisualElement _bottomIcon;
    private Label _tabTitle;
    private string _tabTitleText = "Pending Requests";

    public void GeneratePendingRequestsTab()
    {
        //create tab
        _pendingRequestsTab = _uiUtilities.CreateAndAddToParent<VisualElement>("tab", _requestUI.GetContainer());
        _tabTitle = _uiUtilities.CreateAndAddToParent<Label>("h2", _pendingRequestsTab);
        _uiUtilities.UpdateLabel(_tabTitle, _tabTitleText, "tabTitle");
        _pendingRequestsTab.AddToClassList("hide");

        //create card container
        _cardContainer = new ScrollView(ScrollViewMode.Vertical);
        _cardContainer.AddToClassList("cardContainer");
        _cardContainer.touchScrollBehavior = ScrollView.TouchScrollBehavior.Clamped;
        _pendingRequestsTab.Add(_cardContainer);

        //Debug
        /*var request = new Request
        {
            id = "1",
            timeReceived = "'12:00:00 PM'",
            area = "205",
            guestName = "John",
            details = "2 sets of towels",
            type = "Guest Request",
            receiver = "Houseperson",
            priority = "High",
            submitter = "SP",
            status = "On-going",
            resolution = "",
            timeCompleted = "",
            handler = ""
        };
        _requestCard.GenerateRequestCard(request);*/
    }
    
    

    public VisualElement GetPendingRequestsTab(){ return _pendingRequestsTab;}
    public VisualElement GetCardContainer(){return _cardContainer;}
}
