using UnityEngine;
using UnityEngine.UIElements;

public class RequestCard : MonoBehaviour
{
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private RequestModal _requestModal;
    [SerializeField] private UIUtilities _uiUtilities;
    
    public VisualElement GenerateRequestCard(Request request)
    {
        //request container
        var requestCard = _uiUtilities.CreateAndAddToParent<VisualElement>("requestCard", _requestUI.GetCardContainer());
        
        //area text
        var areaText = _uiUtilities.CreateAndAddToParent<Label>("h3", requestCard);
        _uiUtilities.UpdateLabel(areaText, request.area, "area");
        _uiUtilities.TrimText(areaText, 40);
        
        //request text
        var requestText = _uiUtilities.CreateAndAddToParent<Label>("h4", requestCard);
        requestText.AddToClassList("wrappedText");
        _uiUtilities.UpdateLabel(requestText, request.details, "request");
        _uiUtilities.TrimText(requestText, 85);
            
        //time text + priority tex
        var bottomContainer = _uiUtilities.CreateAndAddToParent<VisualElement>("bottomContainer", requestCard);
        
        var timeText = _uiUtilities.CreateAndAddToParent<Label>("h4", bottomContainer);
        timeText.AddToClassList("grey");
        _uiUtilities.UpdateLabel(timeText, request.timeReceived, "timeReceived");
        
        var priorityText = _uiUtilities.CreateAndAddToParent<Label>("h4", bottomContainer);
        _uiUtilities.UpdateLabel(priorityText, request.priority + " priority", "priority");
        _uiUtilities.SetLabelPriority(priorityText, request.priority);
        
        return requestCard;
    }

    public void UpdateRequestCard(VisualElement requestCard, Request request)
    {
        var areaText = requestCard.Q<Label>(name: "area"); //issue
        if (areaText != null)
            _uiUtilities.UpdateAndTrimText(areaText, 40, request.area);
        
        var requestText = requestCard.Q<Label>(name: "request");
        if (requestText != null)
            _uiUtilities.UpdateAndTrimText(requestText, 85, request.details);
        
        var priorityText = requestCard.Q<Label>(name: "priority");
        if (priorityText != null)
            _uiUtilities.UpdateLabelPriority(priorityText, request.priority);
        
        requestCard.RegisterCallback<ClickEvent>(evt => _requestModal.ShowModal(evt, request));
    }
}
