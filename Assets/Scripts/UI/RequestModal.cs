using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.Networking;

public class RequestModal : MonoBehaviour
{
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private UIUtilities _uiUtilities;
    [SerializeField] private ArchivedRequestsTab _archivedRequestsTab;
    [SerializeField] private PendingRequestsTab _pendingRequestsTab;
    [SerializeField] private RequestManager _requestManager;
    
    //send data to google from
    private string _formUrl = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfsKNFIN2RQIx37zLb0Sj2ynBhfyWqB0Z2zrJhJco6B40wjbw/formResponse";
    
    //modal
    private VisualElement _modalContainer;
    private VisualElement _modal;
    private string _modalPriority;
    private Button _completionBtn;
    private TextField _initialInput;
    private TextField _resolutionInput;
    private string _id;
    private Label _timeReceived;
    private Label _area;
    private Label _guestName;
    private Label _request;
    private Label _logType;
    private Label _priority;
    private Label _requester;
    private Label _status;
    private string _resolution;
    private Label _timeCompleted;
    private Request _currentRequest;
    
    
    public void GenerateRequestModal()
    {
        _modalContainer = _uiUtilities.CreateAndAddToParent<VisualElement>("modalContainer", _requestUI.GetRootComponent());
        _modal = _uiUtilities.CreateAndAddToParent<VisualElement>("modal", _modalContainer);
        
        //close btn
        var closeBtn = _uiUtilities.CreateAndAddToParent<Image>("closeBtn", _modal);
        closeBtn.RegisterCallback<ClickEvent>(evt => HideModal());
        
        //modal title
        var modalTitle = _uiUtilities.CreateAndAddToParent<Label>("h3 upperCenter margin_bottom_sm margin_top_sm", _modal);
        _uiUtilities.UpdateLabel(modalTitle, "Request Details", "modalTitle");
        
        //request details
        var timeReceived = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", _modal);
        _uiUtilities.UpdateLabel(timeReceived, "Time received:", "timeReceivedTitle"); 
        _timeReceived = _uiUtilities.CreateAndAddToParent<Label>("h4 margin_bottom_md", _modal);
        _uiUtilities.UpdateLabel(_timeReceived, "", "timeReceived");
        
        var areaTitle = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", _modal);
        _uiUtilities.UpdateLabel(areaTitle, "Area/Room:", "areaTitle"); 
        _area = _uiUtilities.CreateAndAddToParent<Label>("h4 wrappedText margin_bottom_md", _modal);
        _uiUtilities.UpdateLabel(_area, "", "area");
        
        var guestNameTitle = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", _modal);
        _uiUtilities.UpdateLabel(guestNameTitle, "Guest's name:", "guestNameTitle"); 
        _guestName = _uiUtilities.CreateAndAddToParent<Label>("h4 wrappedText margin_bottom_md", _modal);
        _uiUtilities.UpdateLabel(_guestName, "", "guestName");
        
        var requestTitle = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", _modal);
        _uiUtilities.UpdateLabel(requestTitle, "Request details:", "requestTitle");
        _request = _uiUtilities.CreateAndAddToParent<Label>("h4 wrappedText margin_bottom_md", _modal);
        _uiUtilities.UpdateLabel(_request,"", "request");
        
        var logTypeTitle = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", _modal);
        _uiUtilities.UpdateLabel(logTypeTitle, "Log type:", "logTypeTitle");
        _logType = _uiUtilities.CreateAndAddToParent<Label>("h4 wrappedText margin_bottom_md", _modal);
        _uiUtilities.UpdateLabel(_logType,"", "logType");
        
        var priority = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", _modal);
        _uiUtilities.UpdateLabel(priority, "Priority:", "priorityTitle"); 
        _priority = _uiUtilities.CreateAndAddToParent<Label>("h4 wrappedText margin_bottom_md", _modal);
        _uiUtilities.UpdateLabel(_priority, "", "priority");
        
        var requesterTitle = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", _modal);
        _uiUtilities.UpdateLabel(requesterTitle, "Request logged by:", "requesterTitle");
        _requester = _uiUtilities.CreateAndAddToParent<Label>("h4 wrappedText margin_bottom_md", _modal);
        _uiUtilities.UpdateLabel(_requester,"", "requester");
        
        //resolution input
        var resolutionTitle = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", _modal);
        _uiUtilities.UpdateLabel(resolutionTitle, "Resolution:", "resolutionTitle"); 
        _resolutionInput = _uiUtilities.CreateAndAddToParent<TextField>("textField margin_bottom_md", _modal);
        _resolutionInput.maxLength = 100;
        
        //initial input
        var initialTitle = _uiUtilities.CreateAndAddToParent<Label>("h4 bold", _modal);
        _uiUtilities.UpdateLabel(initialTitle, "Employee's initial:", "initialTitle"); 
        _initialInput = _uiUtilities.CreateAndAddToParent<TextField>("textField margin_bottom_lg", _modal);
        _initialInput.maxLength = 10;
        
        //completion button
        _completionBtn = _uiUtilities.CreateAndAddToParent<Button>("btn", _modal);
        _completionBtn.text = "Complete Request";
        _completionBtn.RegisterCallback<ClickEvent>(_ => CompleteRequest(_currentRequest));
        
        /*var request = new Request
        {
            id = "1",
            timeReceived = "'12:00:00 PM'",
            area = "Discovering new places often leads to unforgettable experiences and personal gro", 
            guestName = "John",
            details = "Discovering new places often leads to unforgettable experiences and personal growth. Every journey teaches us valuable lessons and opens our minds to new possibilities. Embrace the adve",
            type = "Guest Request",
            receiver = "Houseperson",
            priority = "Medium",
            submitter = "SP",
            status = "On-going",
            resolution = "",
            timeCompleted = "",
            handler = ""
        };
        GenerateModalContent(request);*/
    }
    
    private void ValidateInitialInput()
    {
        if (_initialInput.text == "")
            _completionBtn.SetEnabled(false);
        else
            _completionBtn.SetEnabled(true);
    }

    public void ShowModal(Request request)
    {
        _currentRequest = request;
        _modalContainer.AddToClassList("shownModal");
        _modalPriority = request.priority;
        if (_modalPriority == "High")
            _modal.AddToClassList("highPriority");
        else
            _modal.RemoveFromClassList("highPriority");
        
        GenerateModalContent(_currentRequest);
        InvokeRepeating("ValidateInitialInput", 0, 0.5f);
    }

    private void HideModal()
    {
        _modalContainer.RemoveFromClassList("shownModal");
        _modal.RemoveFromClassList("shownModal");
        CancelInvoke("ValidateInitialInput");
    }
    
    private void CompleteRequest(Request request)
    {
        request.status = "Complete";
        request.resolution = _resolutionInput.text;
        request.timeCompleted = _uiUtilities.GetCurrentTime();
        request.handler = _initialInput.text;
        _requestManager.HideRequestCard(request, _pendingRequestsTab.GetCardContainer().Children());
        StartCoroutine(UpdateSheetDataRoutine(request));
        HideModal();
    }
    
    private void GenerateModalContent(Request request)
    {
        _timeReceived.text = request.timeReceived.Substring(1, request.timeReceived.Length -2);
        _timeReceived.AddToClassList("grey");
        
        _area.text = request.area;
        _area.AddToClassList("grey");
        
        _guestName.text = request.guestName;
        _guestName.AddToClassList("grey");
        
        _request.text = request.details;
        _request.AddToClassList("grey");
        
        _logType.text = request.type;
        _logType.AddToClassList("grey");
        
        _priority.text = request.priority;
        _priority.AddToClassList("grey");
        
        _requester.text = request.submitter;
        _requester.AddToClassList("grey");
    }

    IEnumerator UpdateSheetDataRoutine(Request request)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.715305477", request.id);
        form.AddField("entry.530669248", request.status);
        form.AddField("entry.1190988772", request.resolution);
        form.AddField("entry.196062821", request.timeCompleted);
        form.AddField("entry.1883248811", request.handler);

        using (UnityWebRequest www = UnityWebRequest.Post(_formUrl, form))
        {
            yield return www.SendWebRequest();
            if(www.result == UnityWebRequest.Result.Success)
                Debug.Log("Request has been updated on google sheet");
            else 
                Debug.LogError("Error in feedback submission: " + www.error);
        }
    }
}
