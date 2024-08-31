using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class RequestListUI : MonoBehaviour
{
    [SerializeField] private UIDocument _doc;
    [SerializeField] private StyleSheet _styleSheet;
    private ScrollView _requestsContainer;
    
    public void GenerateTaskList()
    {
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

    public void  CreateSingleRequest(string timeReceived, string area, string requestDetails, string priority)
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

        if (priority == "High")
        {
            singleRequestContainer.AddToClassList("highPriority");
        }
            
        //area text
        var areaText = Create<Label>("h3");
        areaText.text = area;
        areaText.text = TrimText(areaText, 40);
        singleRequestContainer.Add(areaText);
        
        //request text
        var requestText = Create<Label>("h4");
        requestText.AddToClassList("wrappedText");
        requestText.text = requestDetails;
        requestText.text = TrimText(requestText, 85);
        singleRequestContainer.Add(requestText);
            
        //time text
        var timeText = Create<Label>("h4");
        timeText.AddToClassList("grey");
        timeText.text = timeReceived;
        singleRequestContainer.Add(timeText);
    }
    
}
