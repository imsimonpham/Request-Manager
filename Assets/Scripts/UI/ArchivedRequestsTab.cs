using UnityEngine;
using UnityEngine.UIElements;

public class ArchivedRequestsTab : MonoBehaviour
{
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private UIUtilities _uiUtilities;
    [SerializeField] private RequestManager _requestManager;
    
    private VisualElement _archiveRequestsTab;
    private ScrollView _cardContainer;
    private Label _tabTitle;
    private string _tabTitleTextBase = "Archived Requests";
    private int _requestCount;
    private string _tabTitleText;
    
    public void GenerateArchivedRequestsTab()
    {
        //create tab
        _archiveRequestsTab = _uiUtilities.CreateAndAddToParent<VisualElement>("tab", _requestUI.GetContainer());
        _tabTitle = _uiUtilities.CreateAndAddToParent<Label>("h2", _archiveRequestsTab);
        _uiUtilities.UpdateLabel(_tabTitle, _tabTitleText, "tabTitle");
        _archiveRequestsTab.AddToClassList("hide");
        
        //create card container
        _cardContainer = new ScrollView(ScrollViewMode.Vertical);
        _cardContainer.AddToClassList("cardContainer");
        _cardContainer.touchScrollBehavior = ScrollView.TouchScrollBehavior.Clamped;
        _archiveRequestsTab.Add(_cardContainer);
        
        //Debug
        /*var request = new Request
        {
            id = "1",
            timeReceived = "'12:00:00 PM'",
            area = "FEDvmMkX34G0I5eqyuzvd81hNIn1wbRbyrs",
            guestName = "John",
            details = "FEDvmMkX34G0I5eqyuzvd81hNIn1wbRbyrs",
            type = "Guest Request",
            receiver = "Houseperson",
            priority = "Medium",
            submitter = "SP",
            status = "On-going",
            resolution = "",
            timeCompleted = "12:10:00 PM",
            handler = ""
        };
        _requestManager.CreateArchivedRequestCard(request);*/
    }
    
    public VisualElement GetArchiveRequestsTab() {return _archiveRequestsTab;}
    public VisualElement GetCardContainer() {return _cardContainer;}
    
    public void UpdateArchivedRequestCountUI(int requestCount)
    {
        _requestCount = requestCount;
        _tabTitleText = _tabTitleTextBase + " (" + _requestCount + ")";
        _uiUtilities.UpdateLabel(_tabTitle == null ? new Label("Archived Requests") : _tabTitle, _tabTitleText, "tabTitle");
    }
}
