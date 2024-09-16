using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIUtilities : MonoBehaviour
{

    public string GetToday()
    {
        DateTime today = DateTime.Now;
        string formattedDate = today.ToString("MMMM dd, yyyy");
        return formattedDate;
    }
    
    public void UpdateLabel(Label label, string text, string labelName)
    {
        label.text = text;
        label.name = labelName;
    }

    public T CreateAndAddToParent<T>(string classNames, VisualElement parent) where T : VisualElement, new()
    {
        var el = new T();
        var classList = classNames.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var className in classList)
        {
            el.AddToClassList(className);
        }
        parent.Add(el);
        return el;
    }
    
    public void TrimText(Label label, int maxLength)
    {
        if (label.text.Length > maxLength)
            label.text = label.text.Substring(0, maxLength) + "...";
    }
    
    public void UpdateAndTrimText(Label label, int maxLength, string text)
    {
        label.text = text;
        if (label.text.Length > maxLength)
            label.text = label.text.Substring(0, maxLength) + "...";
    }

    public void SetLabelPriority(Label priorityText, string priorityData)
    {
        if (priorityData == "High")
        {
            priorityText.parent.parent.AddToClassList("highPriority");
            priorityText.AddToClassList("highPriority");
        }
        else
        {
            priorityText.parent.parent.RemoveFromClassList("highPriority");
            priorityText.AddToClassList("medPriority");
        }
    }

    public void UpdateLabelPriority(Label priorityText, string priorityData)
    {
        priorityText.text = priorityData + " priority";
        if (priorityData == "High")
        {
            priorityText.AddToClassList("highPriority");
            priorityText.RemoveFromClassList("medPriority");
            priorityText.parent.parent.AddToClassList("highPriority");
        }
        else
        {
            priorityText.AddToClassList("medPriority");
            priorityText.RemoveFromClassList("highPriority");
            priorityText.parent.parent.RemoveFromClassList("highPriority");
        }
    }
}
