using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class RequestManager : MonoBehaviour
{
    [SerializeField] private SheetManager _sheetManager;
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private PendingRequestsTab _pendingRequestTab;
    [SerializeField] private ArchivedRequestsTab _archivedRequestsTab;
    [SerializeField] private RequestModal _requestModal;
    [SerializeField] private UIUtilities _uiUtilities;
    
    //dev
    private string _formUrl_HP_Dev = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfsKNFIN2RQIx37zLb0Sj2ynBhfyWqB0Z2zrJhJco6B40wjbw/formResponse";
    
    //prod
    private string _formUrl_HP_Prod = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfYl2L0Bgta-tV42j2ap9G1swfVY8PEYmoY0kcGDio3AAuVng/formResponse";

    public void CreateRequestCard(Request request)
    {
        //request container
        var requestCardClasses = request.priority == "High" ? "requestCard highPriority" : "requestCard";
        var requestCard = _uiUtilities.CreateAndAddToParent<VisualElement>(requestCardClasses, _pendingRequestTab.GetCardContainer());
        requestCard.name = request.id;
        
        requestCard.RegisterCallback<ClickEvent>(_ => _requestModal.ShowModal(request));
        
        //area text
        var areaText = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", requestCard);
        _uiUtilities.UpdateLabel(areaText, request.area, "area");
        _uiUtilities.TrimText(areaText, 35);
        
        //request text
        var requestText = _uiUtilities.CreateAndAddToParent<Label>("h4 wrappedText", requestCard);
        _uiUtilities.UpdateLabel(requestText, request.details, "request");
        _uiUtilities.TrimText(requestText, 40);
            
        //time text + priority tex
        var bottomContainer = _uiUtilities.CreateAndAddToParent<VisualElement>("bottomContainer", requestCard);
        
        var timeReceived = _uiUtilities.CreateAndAddToParent<Label>("h4 grey", bottomContainer);
        _uiUtilities.UpdateLabel(timeReceived, request.timeReceived, "timeReceived");
        
        var priorityText = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", bottomContainer);
        var priority = request.priority == "High" ? "High priority" : "";
        _uiUtilities.UpdateLabel(priorityText, priority, "priority");
    }
    
    public void CreateArchivedRequestCard(Request request)
    {
        //request container
        var archivedRequestCard = _uiUtilities.CreateAndAddToParent<VisualElement>("requestCard archived", _archivedRequestsTab.GetCardContainer());
        var leftCol = _uiUtilities.CreateAndAddToParent<VisualElement>("leftCol", archivedRequestCard);
        var rightCol = _uiUtilities.CreateAndAddToParent<VisualElement>("rightCol", archivedRequestCard);
        archivedRequestCard.name = request.id;
        
        //undo button
        var restoreBtn = _uiUtilities.CreateAndAddToParent<Image>("restoreBtn", rightCol);
        rightCol.RegisterCallback<ClickEvent>(evt => RestoreRequest(request));
        
        //area text
        var areaText = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", leftCol);
        _uiUtilities.UpdateLabel(areaText, request.area, "area");
        _uiUtilities.TrimText(areaText, 20);
        
        //request text
        var requestText = _uiUtilities.CreateAndAddToParent<Label>("h4 wrappedText", leftCol);
        _uiUtilities.UpdateLabel(requestText, request.details, "request");
        _uiUtilities.TrimText(requestText, 25);
            
        //time text + priority text
        var bottomContainer = _uiUtilities.CreateAndAddToParent<VisualElement>("bottomContainer", leftCol);
        
        var timeReceived = _uiUtilities.CreateAndAddToParent<Label>("h4 grey", bottomContainer);
        _uiUtilities.UpdateLabel(timeReceived, request.timeReceived, "timeReceived");
        
        var timeCompleted = _uiUtilities.CreateAndAddToParent<Label>("h4 grey", bottomContainer);
        _uiUtilities.UpdateLabel(timeCompleted, _uiUtilities.GetCurrentTime(), "timeCompleted");
    }

    public void UpdateRequestCard(Request request, PendingRequestsTab pendingRequestsTab, List<Request> requestList = null)
    {
        foreach (VisualElement card in pendingRequestsTab.GetCardContainer().Children())
        {
            if (card.name == request.id)
            {
                UpdateRequestCardData(request, card);
                break;
            }
        }

        // Debug: Update the request in the list
        requestList ??= new List<Request>();
        int index = requestList.FindIndex(r => r.id == request.id);
        if (index != -1)
        {
            requestList[index] = request;
        }
    }

    private void UpdateRequestCardData(Request request, VisualElement requestCard)
    {
        var areaText = requestCard.Q<Label>(name: "area"); 
        if (areaText != null)
            _uiUtilities.UpdateAndTrimText(areaText, 35, request.area);
        
        var requestText = requestCard.Q<Label>(name: "request");
        if (requestText != null)
            _uiUtilities.UpdateAndTrimText(requestText, 40, request.details);
        
        var priorityText = requestCard.Q<Label>(name: "priority");
        if (priorityText != null)
            _uiUtilities.UpdateLabelPriority(priorityText, request.priority);
    }
    
    public void AddRequest(Request request, Dictionary<string, Request> requestDict, List<Request> requestList = null)
    {
        requestList ??= new List<Request>();
        
        requestDict.Add(request.id, request); 
        requestList.Add(request);
        CreateRequestCard(request);
    }

    public void RemoveRequest(Request request, Dictionary<string, Request> requestDict,  PendingRequestsTab pendingRequestsTab,  List<Request> requestList = null)
    {
        foreach (VisualElement card in pendingRequestsTab.GetCardContainer().Children())
        {
            if (card.name == request.id)
            {
                pendingRequestsTab.GetCardContainer().Remove(card);
                requestDict.Remove(request.id);
                break;
            }
        }
        requestList ??= new List<Request>();
        int index = requestList.FindIndex(r => r.id == request.id);
        if (index != -1)
            requestList.RemoveAt(index);
    }
    
    public void RemoveAllPendingRequests(Dictionary<string, Request> requestDict, PendingRequestsTab pendingRequestsTab, List<Request> requestList = null)
    {
        pendingRequestsTab.GetCardContainer().Clear();
        requestDict.Clear();
        requestList ??= new List<Request>();
        requestList.Clear();
    }
    
    public void AddArchivedRequest(Request request,  Dictionary<string, Request> archivedRequestDict,  List<Request> archivedRequestList = null)
    {
        if (!archivedRequestDict.ContainsKey(request.id))
        {
            archivedRequestList ??= new List<Request>();
            
            archivedRequestDict.Add(request.id, request);
            archivedRequestList.Add(request);
            CreateArchivedRequestCard(request);
        }
    }
    
    private void RestoreRequest(Request request)
    {
        request.status = "On-going";
        request.timeCompleted = "";
        request.handler = "";
        request.isViewed = "Viewed";
        HideRequestCard(request, _archivedRequestsTab.GetCardContainer().Children());
        StartCoroutine(RestoreRequestRoutine(request));
    }
    
    public void RemoveArchivedRequest(Request request, Dictionary<string, Request> archivedRequestDict, ArchivedRequestsTab archivedRequestsTab, List<Request> archivedrequestList = null)
    {
        foreach (VisualElement card in archivedRequestsTab.GetCardContainer().Children())
        {
            if (card.name == request.id)
            {
                archivedRequestsTab.GetCardContainer().Remove(card);
                archivedRequestDict.Remove(request.id);
                break;
            }
        }
        
        archivedrequestList ??= new List<Request>();
        int index = archivedrequestList.FindIndex(r => r.id == request.id);
        if (index != -1)
            archivedrequestList.RemoveAt(index);
    }
    
    public void RemoveAllArchivedRequests(Dictionary<string, Request> archivedRequestDict, ArchivedRequestsTab archivedRequestsTab, List<Request> archivedrequestList = null)
    {
        archivedRequestsTab.GetCardContainer().Clear();
        archivedRequestDict.Clear();
        archivedrequestList ??= new List<Request>();
        archivedrequestList.Clear();
    }
    
    public void HideRequestCard(Request request, IEnumerable<VisualElement> cardCollection)
    {
        foreach (VisualElement card in cardCollection)
        {
            if (card.name == request.id)
            {
                card.AddToClassList("hide");
                break;
            }
        }
    }

    IEnumerator RestoreRequestRoutine(Request request)
    {
        WWWForm form = new WWWForm();
        form.AddField(_sheetManager.IsDev() ? "entry.715305477" : "entry.1210669559", request.id);
        form.AddField(_sheetManager.IsDev() ? "entry.530669248" : "entry.225830786", request.status);
        form.AddField(_sheetManager.IsDev() ? "entry.1190988772" : "entry.1907341931", request.notes);
        form.AddField(_sheetManager.IsDev() ? "entry.196062821" : "entry.2117647377", request.timeCompleted);
        form.AddField(_sheetManager.IsDev() ? "entry.1883248811" : "entry.383433174", request.handler);
        form.AddField(_sheetManager.IsDev() ? "entry.1142493925" : "entry.844543065", request.isViewed);

        using (UnityWebRequest www = UnityWebRequest.Post(_sheetManager.IsDev() ? _formUrl_HP_Dev : _formUrl_HP_Prod, form))
        {
            yield return www.SendWebRequest();
            if(www.result == UnityWebRequest.Result.Success)
                Debug.Log("Request has been updated on google sheet");
            else 
                Debug.LogError("Error in feedback submission: " + www.error);
        }
    }
}
