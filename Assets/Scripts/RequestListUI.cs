using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class RequestListUI : MonoBehaviour
{
    [SerializeField] private UIDocument _doc;
    [SerializeField] private StyleSheet _styleSheet;
    private VisualElement _root;
    private VisualElement _container;
    private ScrollView _requestCardContainer;
    private VisualElement _modalContainer;
    private VisualElement _modal;
    private string _modalPriority;
    private Button _completionBtn;
    private TextField _initialInput;
    public void GenerateTaskList()
    {
        StartCoroutine(GenerateUI());
    }
    
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        StartCoroutine(GenerateUI());
    }

    private IEnumerator GenerateUI()
    {
        yield return null;
        _root = _doc.rootVisualElement;
        _root.Clear();
        _root.styleSheets.Add(_styleSheet);

        //container
        _container = CreateAndAddToParent<VisualElement>("container", _root);
        
        //tab title
        var title = CreateAndAddToParent<Label>("h1", _container);
        title.AddToClassList("center");
        title.AddToClassList("title");
        UpdateLabel(title, "Today", "title");
        
        //task list title
        var requestText = CreateAndAddToParent<Label>("h2", _container);
        UpdateLabel(requestText, "Pending Requests", "subtitle");
        
        //task collection container
        _requestCardContainer = new ScrollView(ScrollViewMode.Vertical);
        _requestCardContainer.AddToClassList("requestContainer");
        _requestCardContainer.touchScrollBehavior = ScrollView.TouchScrollBehavior.Elastic;
        _container.Add(_requestCardContainer);
        
        /*VisualElement requestUI = CreateSingleRequest("10:46:01 AM", "936", "Power to please", "Medium");*/
        GenerateRequestModal("", "", "");
    }
    
    #region Request Card
    
    public VisualElement  CreateRequestCard(string timeReceived, string area, string requestDetails, string priority)
    {
        //request container
        var requestCard = CreateAndAddToParent<VisualElement>("requestCard", _container);
        requestCard.RegisterCallback<ClickEvent>(evt => ShowModal(evt, priority));
        
        //area text
        var areaText = CreateAndAddToParent<Label>("h3", requestCard);
        UpdateLabel(areaText, area, "area");
        TrimText(areaText, 40);
        
        //request text
        var requestText = CreateAndAddToParent<Label>("h4", requestCard);
        requestText.AddToClassList("wrappedText");
        UpdateLabel(requestText, requestDetails, "request");
        TrimText(requestText, 85);
            
        //time text + priority tex
        var bottomContainer = CreateAndAddToParent<VisualElement>("bottomContainer", requestCard);
        
        var timeText = CreateAndAddToParent<Label>("h4", bottomContainer);
        timeText.AddToClassList("grey");
        UpdateLabel(timeText, timeReceived, "timeReceived");
        
        var priorityText = CreateAndAddToParent<Label>("h4", bottomContainer);
        UpdateLabel(priorityText, priority + " priority", "priority");
        SetPriorityUI(priorityText, priority);
        
        return requestCard;
    }

    public void UpdateRequestCard(VisualElement request, string area, string requestDetails, string priority)
    {
        var areaText = request.Q<Label>(name: "area");
        if (areaText != null)
            UpdateAndTrimText(areaText, 40, area);
        
        var requestText = request.Q<Label>(name: "request");
        if (requestText != null)
            UpdateAndTrimText(requestText, 85, requestDetails);
        
        var priorityText = request.Q<Label>(name: "priority");
        if (priorityText != null)
            UpdatePriorityUI(priorityText, priority);
    }
    
    #endregion
    
    #region Request Modal
    
    private void GenerateRequestModal(string timeReceived, string area, string requestDetails)
    {
        _modalContainer = CreateAndAddToParent<VisualElement>("modalContainer", _root);
        /*_modalContainer.RegisterCallback<ClickEvent>(evt => HideModal(evt));*/
        _modal = CreateAndAddToParent<VisualElement>("modal", _modalContainer);
        
        //close btn
        var closeBtn = CreateAndAddToParent<Image>("closeBtn", _modal);
        closeBtn.RegisterCallback<ClickEvent>(evt => HideModal(evt));
        
        //modal title
        var modalTitle = CreateAndAddToParent<Label>("h2", _modal);
        modalTitle.AddToClassList("center");
        UpdateLabel(modalTitle, "Request Details", "modalTitle");
        
        //request details
        var timeTitle = CreateAndAddToParent<Label>("h3", _modal);
        UpdateLabel(timeTitle, "Time received:", "timeTitle");
        var timeText = CreateAndAddToParent<Label>("h4", _modal);
        timeText.AddToClassList("margin_bottom_lg");
        UpdateLabel(timeText, timeReceived, "timeReceived");
        
        var areaTitle = CreateAndAddToParent<Label>("h3", _modal);
        UpdateLabel(areaTitle, "Area/Room:", "areaTitle"); 
        var areaText = CreateAndAddToParent<Label>("h4", _modal);
        areaText.AddToClassList("margin_bottom_lg");
        UpdateLabel(areaText, area, "area");
        
        var requestTitle = CreateAndAddToParent<Label>("h3", _modal);
        UpdateLabel(requestTitle, "Request details:", "requestTitle");
        var request = CreateAndAddToParent<Label>("h4", _modal);
        request.AddToClassList("margin_bottom_lg");
        UpdateLabel(request, requestDetails, "request");
        
        //initial input
        var initialTitle = CreateAndAddToParent<Label>("h3", _modal);
        UpdateLabel(initialTitle, "Employee's initial:", "initialTitle"); 
        _initialInput = CreateAndAddToParent<TextField>("textField", _modal);
        _initialInput.AddToClassList("margin_bottom_lg");
        _initialInput.maxLength = 10;
        
        //completion button
        _completionBtn = CreateAndAddToParent<Button>("btn", _modal);
        _completionBtn.text = "Complete Request";
        _completionBtn.RegisterCallback<ClickEvent>(evt => HideModal(evt));
        
        InvokeRepeating("ValidateInitialInput", 0, 1);
        
    }

    private void ValidateInitialInput()
    {
        if (_initialInput == null)
        {
            Debug.Log("null");
            return;
        }
        if(_initialInput.text == "")
        {
            _completionBtn.SetEnabled(false);
        }
        else
        {
            _completionBtn.SetEnabled(true);
        }
    }

    private void ShowModal(ClickEvent evt, string priority)
    {
        _modalContainer.AddToClassList("shownModal");
        _modal.AddToClassList("shownModal");
        _modalPriority = priority;
        if (_modalPriority == "High")
        {
            _modal.AddToClassList("highPriority");
            _completionBtn.AddToClassList("highpriority");
        }
        else
        {
            _modal.RemoveFromClassList("highPriority");
            _completionBtn.RemoveFromClassList("highPriority");
        }
    }

    private void HideModal(ClickEvent evt)
    {
        _modalContainer.RemoveFromClassList("shownModal");
        _modal.RemoveFromClassList("shownModal");
    }
    
    #endregion
    
    #region Utility Functions
    void UpdateLabel(Label label, string text, string labelName)
    {
        label.text = text;
        label.name = labelName;
    }

    T CreateAndAddToParent<T>(string className, VisualElement parent) where T : VisualElement, new()
    {
        var el = new T();
        el.AddToClassList(className);
        parent.Add(el);
        return el;
    }
    
    void TrimText(Label label, int maxLength)
    {
        if (label.text.Length > maxLength)
            label.text = label.text.Substring(0, maxLength) + "...";
    }
    
    void UpdateAndTrimText(Label label, int maxLength, string text)
    {
        label.text = text;
        if (label.text.Length > maxLength)
            label.text = label.text.Substring(0, maxLength) + "...";
    }

    void SetPriorityUI(Label priorityText, string priorityData)
    {
        if (priorityData == "High")
        {
            priorityText.parent.parent.AddToClassList("highPriority");
            priorityText.AddToClassList("red");
        }
        else
        {
            priorityText.parent.parent.RemoveFromClassList("highPriority");
            priorityText.AddToClassList("yellow");
            /*_modal.AddToClassList("shownModal");*/
        }
    }

    void UpdatePriorityUI(Label priorityText, string priorityData)
    {
        priorityText.text = priorityData + " priority";
        if (priorityData == "High")
        {
            priorityText.AddToClassList("red");
            priorityText.RemoveFromClassList("yellow");
            priorityText.parent.parent.AddToClassList("highPriority");
            /*_modal.AddToClassList("highPriority");*/
        }
        else
        {
            priorityText.AddToClassList("yellow");
            priorityText.RemoveFromClassList("red");
            priorityText.parent.parent.RemoveFromClassList("highPriority");
            /*_modal.RemoveFromClassList("highPriority");*/
        }
    }
    
    #endregion
}
