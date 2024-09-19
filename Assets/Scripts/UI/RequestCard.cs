using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RequestCard : MonoBehaviour
{
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private PendingRequestsTab _pendingRequestTab;
    [SerializeField] private ArchivedRequestsTab _archivedRequestsTab;
    [SerializeField] private RequestModal _requestModal;
    [SerializeField] private UIUtilities _uiUtilities;

    public VisualElement GenerateRequestCard(Request request)
    {
        //request container
        var requestCardClasses = request.priority == "High" ? "requestCard highPriority" : "requestCard";
        var requestCard = _uiUtilities.CreateAndAddToParent<VisualElement>(requestCardClasses, _pendingRequestTab.GetCardContainer());
        
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
        _uiUtilities.UpdateLabel(timeReceived, request.timeReceived.Substring(1, request.timeReceived.Length -2), "timeReceived");
        
        var priorityText = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", bottomContainer);
        var priority = request.priority == "High" ? "High priority" : "";
        _uiUtilities.UpdateLabel(priorityText, priority, "priority");
        
        return requestCard;
    }
    
    public VisualElement GenerateArchivedRequestCard(Request request)
    {
        //request container
        var archivedRequestCard = _uiUtilities.CreateAndAddToParent<VisualElement>("requestCard complete", _archivedRequestsTab.GetCardContainer());
        
        //area text
        var areaText = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", archivedRequestCard);
        _uiUtilities.UpdateLabel(areaText, request.area, "area");
        _uiUtilities.TrimText(areaText, 35);
        
        //request text
        var requestText = _uiUtilities.CreateAndAddToParent<Label>("h4 wrappedText", archivedRequestCard);
        _uiUtilities.UpdateLabel(requestText, request.details, "request");
        _uiUtilities.TrimText(requestText, 40);
            
        //time text + priority tex
        var bottomContainer = _uiUtilities.CreateAndAddToParent<VisualElement>("bottomContainer", archivedRequestCard);
        
        var timeReceived = _uiUtilities.CreateAndAddToParent<Label>("h4 grey", bottomContainer);
        _uiUtilities.UpdateLabel(timeReceived, request.timeReceived.Substring(1, request.timeReceived.Length -2), "timeReceived");
        
        var timeCompleted = _uiUtilities.CreateAndAddToParent<Label>("h4 grey", bottomContainer);
        _uiUtilities.UpdateLabel(timeCompleted, request.timeCompleted, "timeCompleted");
        
        return archivedRequestCard;
    }

    public void UpdateRequestCard(VisualElement requestCard, Request request)
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
}
