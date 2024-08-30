using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class TaskListUI : MonoBehaviour
{
    [SerializeField] private UIDocument _doc;
    [SerializeField] private StyleSheet _styleSheet;
    [SerializeField] private VisualElement _container;

    private void Start()
    {
        
    }
    
    private void OnValidate()
    {
        /*if (Application.isPlaying) return;
        StartCoroutine(Generate());*/
    }

    public void GenerateTaskList(string area, string timeReceived, string request, string priority, string notification)
    {
        StartCoroutine(Generate(area, timeReceived, request, priority, notification));
    }

    public IEnumerator Generate(string area, string timeReceived, string request, string priority, string notification)
    {
        yield return null;
        var root = _doc.rootVisualElement;
        root.Clear();
        
        root.styleSheets.Add(_styleSheet);

        _container = Create("container");
        root.Add(_container);
        
        //app title
        var title = Create<Label>("h1");
        title.AddToClassList("title");
        title.text = "Today";
        _container.Add(title);
        
        //task list title
        var taskText = Create<Label>("h2");
        taskText.text = "Pending Tasks";
        _container.Add(taskText);
        
        //task collection container
        var taskContainer = new ScrollView(ScrollViewMode.Vertical);
        taskContainer.AddToClassList("taskContainer");
        taskContainer.touchScrollBehavior = ScrollView.TouchScrollBehavior.Elastic;
        _container.Add(taskContainer);

        if (notification == "Sent")
        {
            VisualElement task = CreateSingleTask(area, timeReceived, request, priority);
            taskContainer.Add(task);
        }
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

    public VisualElement CreateSingleTask(string area, string timeReceived, string request, string priority)
    {
        //task container
        var singleTaskContainer = Create("singleTaskContainer");
        /*var btnContainer = Create("btnContainer");*/
        var requestContainer = Create("requestContainer");
        /*if (_container != null)
        {
            _container.Add(singleTaskContainer);
        }
        else
        {
            Debug.LogError("container is null");
        }*/
        
        /*singleTaskContainer.Add(btnContainer);*/
        singleTaskContainer.Add(requestContainer);

        if (priority == "High")
        {
            singleTaskContainer.AddToClassList("highPriority");
        }
            
        //completion button
        /*var completionButton = Create<Button>("completionButton");
        btnContainer.Add(completionButton);*/
            
        //area text
        var areaText = Create<Label>("h3");
        areaText.text = area;
        areaText.text = TrimText(areaText, 40);
        requestContainer.Add(areaText);
        
        //request text
        var requestText = Create<Label>("h4");
        requestText.AddToClassList("wrappedText");
        requestText.text = request;
        requestText.text = TrimText(requestText, 85);
        requestContainer.Add(requestText);
            
        //time text
        var timeText = Create<Label>("h4");
        timeText.AddToClassList("grey");
        timeText.text = timeReceived;
        requestContainer.Add(timeText);

        return singleTaskContainer;
    }
    
}
