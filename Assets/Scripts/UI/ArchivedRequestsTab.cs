using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class ArchivedRequestsTab : MonoBehaviour
{
    [Header("Request Data")] 
    [SerializeField] private RequestCard _requestCard; //debug
    private Dictionary<string, Request> _archivedRequestDict = new Dictionary<string, Request>();
    [SerializeField] private List<Request> _archivedRequestList= new List<Request>(); //debug
    
    [Header("UI")] 
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private UIUtilities _uiUtilities;
    private Dictionary<string, VisualElement> _archivedRequestUIDict = new Dictionary<string, VisualElement>();
    
    private VisualElement _archiveRequestsTab;
    private ScrollView _cardContainer;
    private VisualElement _bottomIcon;
    private Label _tabTitle;
    private string _tabTitleText = "Archived Requests";
    
    
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
            area = "205",
            guestName = "John",
            details = "4 sets of towels",
            type = "Guest Request",
            receiver = "Houseperson",
            priority = "Medium",
            submitter = "SP",
            status = "On-going",
            resolution = "",
            timeCompleted = "12:10:00 PM",
            handler = ""
        };
        _requestCard.GenerateArchivedRequestCard(request);*/
    }

    public void AddArchivedRequest(Request request)
    {
        if (!_archivedRequestDict.ContainsKey(request.id))
        {
            _archivedRequestList.Add(request);
            _archivedRequestDict.Add(request.id, request);
        }
    }

    public void AddArchivedRequestCard(Request request)
    {
        if (!_archivedRequestUIDict.ContainsKey(request.id))
        {
            VisualElement card = _requestCard.GenerateArchivedRequestCard(request);
            _archivedRequestUIDict.Add(request.id, card);
        }
    }
    
    public VisualElement GetArchiveRequestsTab() {return _archiveRequestsTab;}
    public VisualElement GetCardContainer() {return _cardContainer;}
}
