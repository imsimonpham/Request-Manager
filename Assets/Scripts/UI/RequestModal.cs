using UnityEngine;
using UnityEngine.UIElements;

public class RequestModal : MonoBehaviour
{
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private UIUtilities _uiUtilities;
    
    //modal
    private VisualElement _modalContainer;
    private VisualElement _modal;
    private string _modalPriority;
    private Button _completionBtn;
    private TextField _initialInput;
    private string _id;
    private Label _timeText;
    private Label _areaText;
    private Label _request;
    private Label _priority;
    private string _initial;
    
    public void GenerateRequestModal()
    {
        _modalContainer = _uiUtilities.CreateAndAddToParent<VisualElement>("modalContainer", _requestUI.GetRootComponent());
        _modal = _uiUtilities.CreateAndAddToParent<VisualElement>("modal", _modalContainer);
        
        //close btn
        var closeBtn = _uiUtilities.CreateAndAddToParent<Image>("closeBtn", _modal);
        closeBtn.RegisterCallback<ClickEvent>(evt => HideModal(evt));
        
        //modal title
        var modalTitle = _uiUtilities.CreateAndAddToParent<Label>("h2", _modal);
        modalTitle.AddToClassList("upperCenter");
        _uiUtilities.UpdateLabel(modalTitle, "Request Details", "modalTitle");
        
        
        //request details
        var timeTitle = _uiUtilities.CreateAndAddToParent<Label>("h3", _modal);
        _uiUtilities.UpdateLabel(timeTitle, "Time received:", "timeTitle");
        _timeText = _uiUtilities.CreateAndAddToParent<Label>("h4", _modal);
        _timeText.AddToClassList("margin_bottom_lg");
        _uiUtilities.UpdateLabel(_timeText, "", "timeReceived");
        
        var areaTitle = _uiUtilities.CreateAndAddToParent<Label>("h3", _modal);
        _uiUtilities.UpdateLabel(areaTitle, "Area/Room:", "areaTitle"); 
        _areaText = _uiUtilities.CreateAndAddToParent<Label>("h4", _modal);
        _areaText.AddToClassList("margin_bottom_lg");
        _uiUtilities.UpdateLabel(_areaText, "", "area");
        
        var requestTitle = _uiUtilities.CreateAndAddToParent<Label>("h3", _modal);
        _uiUtilities.UpdateLabel(requestTitle, "Request details:", "requestTitle");
        _request = _uiUtilities.CreateAndAddToParent<Label>("h4", _modal);
        _request.AddToClassList("margin_bottom_lg");
        _uiUtilities.UpdateLabel(_request,"", "request");
        
        var priorityTitle = _uiUtilities.CreateAndAddToParent<Label>("h3", _modal);
        _uiUtilities.UpdateLabel(priorityTitle, "Priority:", "timeTitle");
        _priority = _uiUtilities.CreateAndAddToParent<Label>("h4", _modal);
        _priority.AddToClassList("margin_bottom_lg");
        _uiUtilities.UpdateLabel(_priority, "", "timeReceived");
        
        //initial input
        var initialTitle = _uiUtilities.CreateAndAddToParent<Label>("h3", _modal);
        _uiUtilities.UpdateLabel(initialTitle, "Employee's initial:", "initialTitle"); 
        _initialInput = _uiUtilities.CreateAndAddToParent<TextField>("textField", _modal);
        _initialInput.AddToClassList("margin_bottom_lg");
        _initialInput.maxLength = 10;
        _initial = _initialInput.text;
        
        //completion button
        _completionBtn = _uiUtilities.CreateAndAddToParent<Button>("btn", _modal);
        _completionBtn.text = "Complete Request";
        _completionBtn.RegisterCallback<ClickEvent>(evt => HideModal(evt));
    }

    private void ValidateInitialInput()
    {
        if(_initialInput.text == "")
            _completionBtn.SetEnabled(false);
        else
            _completionBtn.SetEnabled(true);
    }

    public void ShowModal(ClickEvent evt, Request request)
    {
        _modalContainer.AddToClassList("shownModal");
        _modal.AddToClassList("shownModal");
        _modalPriority = request.priority;
        if (_modalPriority == "High")
        {
            _modal.AddToClassList("highPriority");
            _completionBtn.AddToClassList("highPriority");
        }
        else
        {
            _modal.RemoveFromClassList("highPriority");
            _completionBtn.RemoveFromClassList("highPriority");
        }
        
        GenerateModalContent(request);
        InvokeRepeating("ValidateInitialInput", 0, 1);
    }

    private void HideModal(ClickEvent evt)
    {
        _modalContainer.RemoveFromClassList("shownModal");
        _modal.RemoveFromClassList("shownModal");
        CancelInvoke("ValidateInitialInput");
    }

    private void GenerateModalContent(Request request)
    {
        _timeText.text = request.timeReceived;
        _timeText.AddToClassList("grey");
        _areaText.text = request.area;
        _areaText.AddToClassList("grey");
        _request.text = request.details;
        _request.AddToClassList("grey");
        _priority.text = request.priority;

        if (request.priority == "High")
        {
            _priority.AddToClassList("highPriority");
            _priority.RemoveFromClassList("medPriority");
        }
        else
        {
            _priority.AddToClassList("medPriority");
            _priority.RemoveFromClassList("highPriority");
        }
    }
    
}
