using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class RequestListUI : MonoBehaviour
{
    [SerializeField] private UIDocument _doc;
    [SerializeField] private StyleSheet _styleSheet;
    private ScrollView _requestsContainer;
    
    public void GenerateTaskList()
    {
        StartCoroutine(Generate());
    }
    
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        yield return null;
        var root = _doc.rootVisualElement;
        root.Clear();
        
        root.styleSheets.Add(_styleSheet);

        //container
        var container = CreateAndAddToParent<VisualElement>("container", root);
        
        //tab title
        var title = CreateAndAddToParent<Label>("h1", container);
        title.AddToClassList("title");
        UpdateLabel(title, "Today", "title");
        
        //task list title
        var requestText = CreateAndAddToParent<Label>("h2", container);
        UpdateLabel(requestText, "Pending Requests", "subtitle");
        
        //task collection container
        _requestsContainer = new ScrollView(ScrollViewMode.Vertical);
        _requestsContainer.AddToClassList("taskContainer");
        _requestsContainer.touchScrollBehavior = ScrollView.TouchScrollBehavior.Elastic;
        container.Add(_requestsContainer);
        
        /*VisualElement requestUI = CreateSingleRequest("10:46:01 AM", "936", "Power to please", "Medium");*/
    }

    
    public VisualElement  CreateRequestCard(string timeReceived, string area, string requestDetails, string priority)
    {
        //task container
        var requestCard = CreateAndAddToParent<VisualElement>("requestCard", _requestsContainer);
        
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
        SetPriorityUI(priority, priorityText);
        
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
            UpdatePriority(priority, priorityText);
    }
    
    #region Utility Functions

    void UpdateLabel(Label label, string text, string labelName)
    {
        label.text = text;
        label.name = labelName;
    }
    
    T Create<T>(string className) where T : VisualElement, new()
    {
        var el = new T();
        el.AddToClassList(className);
        return el;
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

    void SetPriorityUI(string priorityData, Label priorityText)
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
        }
    }

    void UpdatePriority(string priorityData, Label priorityText)
    {
        priorityText.text = priorityData + " priority";
        if (priorityData == "High")
        {
            priorityText.AddToClassList("red");
            priorityText.RemoveFromClassList("yellow");
            priorityText.parent.parent.AddToClassList("highPriority");
        }
        else
        {
            priorityText.AddToClassList("yellow");
            priorityText.RemoveFromClassList("red");
            priorityText.parent.parent.RemoveFromClassList("highPriority");
        }
    }
    
    #endregion
}
