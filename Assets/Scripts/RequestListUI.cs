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

    public IEnumerator Generate()
    {
        yield return null;
        var root = _doc.rootVisualElement;
        root.Clear();
        
        root.styleSheets.Add(_styleSheet);

        var container = Create("container");
        root.Add(container);
        
        //app title
        var title = Create<Label>("h1");
        title.AddToClassList("title");
        title.text = "Today";
        container.Add(title);
        
        //task list title
        var requestText = Create<Label>("h2");
        requestText.text = "Pending Requests";
        container.Add(requestText);
        
        //task collection container
        _requestsContainer = new ScrollView(ScrollViewMode.Vertical);
        _requestsContainer.AddToClassList("taskContainer");
        _requestsContainer.touchScrollBehavior = ScrollView.TouchScrollBehavior.Elastic;
        container.Add(_requestsContainer);
        
        /*VisualElement requestUI = CreateSingleRequest("10:46:01 AM", "936", "Power to please", "Medium");*/
    }

    VisualElement Create(string className) {return Create<VisualElement>(className);}

    T Create<T>(string className) where T : VisualElement, new()
    {
        var el = new T();
        el.AddToClassList(className);
        return el;
    }
    
    string TrimText(Label label, int maxLength)
    {
        if (label.text.Length <= maxLength) {return label.text;}
        return label.text.Substring(0, maxLength) + "...";
    }

    public VisualElement  CreateSingleRequest(string timeReceived, string area, string requestDetails, string priority)
    {
        //task container
        var singleRequestContainer = Create("singleRequestContainer");
        if (_requestsContainer != null)
        {
            _requestsContainer.Add(singleRequestContainer);
        }
        else
        {
            Debug.LogError("container is null");
        }
            
        //area text
        var areaText = Create<Label>("h3");
        areaText.name = "area";
        areaText.text = area;
        areaText.text = TrimText(areaText, 40);
        singleRequestContainer.Add(areaText);
        
        //request text
        var requestText = Create<Label>("h4");
        requestText.name = "request";
        requestText.AddToClassList("wrappedText");
        requestText.text = requestDetails;
        requestText.text = TrimText(requestText, 85);
        singleRequestContainer.Add(requestText);
            
        //time text + priority tex
        var bottomContainer = Create("bottomContainer");
        var timeText = Create<Label>("h4");
        var priorityText = Create<Label>("h4");
        priorityText.name = "priority";
        timeText.AddToClassList("grey");
        timeText.text = timeReceived;
        priorityText.text = priority + " priority";
        
        if (priority == "High")
        {
            singleRequestContainer.AddToClassList("highPriority");
            priorityText.AddToClassList("red");
            /*priorityText.RemoveFromClassList("yellow");*/
        }
        else
        {
            singleRequestContainer.RemoveFromClassList("highPriority");
            priorityText.AddToClassList("yellow");
            /*priorityText.RemoveFromClassList("red");*/
        }
        
        bottomContainer.Add(timeText);
        bottomContainer.Add(priorityText);
        singleRequestContainer.Add(bottomContainer);

        return singleRequestContainer;
    }

    public void UpdateSingleRequest(VisualElement request, string area, string requestDetails, string priority)
    {
        var areaText = request.Q<Label>(name: "area");
        if (areaText != null)
        {
            areaText.text = area;
            areaText.text = TrimText(areaText, 40);
        }
        
        var requestText = request.Q<Label>(name: "request");
        if (requestText != null)
        {
            requestText.text = requestDetails;
            requestText.text = TrimText(requestText, 85);
        }
        
        var priorityText = request.Q<Label>(name: "priority");
        if (priorityText != null)
        {
            priorityText.text = priority + " priority";

            if (priority == "High")
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
    }
    
}
