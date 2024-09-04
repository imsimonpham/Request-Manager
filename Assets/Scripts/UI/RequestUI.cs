using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class RequestUI : MonoBehaviour
{
    [SerializeField] private UIDocument _doc;
    [SerializeField] private StyleSheet _styleSheet;
    [SerializeField] private UIUtilities _uiUtilities;
    [SerializeField] private RequestModal _requestModal;
    private VisualElement _root;
    private VisualElement _container;
    private ScrollView _requestCardContainer;
    
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        StartCoroutine(GenerateUI());
    }

    public IEnumerator GenerateUI()
    {
        yield return null;
        _root = _doc.rootVisualElement;
        _root.Clear();
        _root.styleSheets.Add(_styleSheet);

        //container
        _container = _uiUtilities.CreateAndAddToParent<VisualElement>("container", _root);
        
        //tab title
        var title = _uiUtilities.CreateAndAddToParent<Label>("h1", _container);
        title.AddToClassList("upperCenter");
        title.AddToClassList("title");
        _uiUtilities.UpdateLabel(title, "Today", "title");
        
        //task list title
        var requestText = _uiUtilities.CreateAndAddToParent<Label>("h2", _container);
        _uiUtilities.UpdateLabel(requestText, "Pending Requests", "subtitle");
        
        //task collection container
        _requestCardContainer = new ScrollView(ScrollViewMode.Vertical);
        _requestCardContainer.AddToClassList("requestContainer");
        _requestCardContainer.touchScrollBehavior = ScrollView.TouchScrollBehavior.Clamped;
        _container.Add(_requestCardContainer);
        
        /*VisualElement requestUI = CreateSingleRequest("10:46:01 AM", "936", "Power to please", "Medium");*/
        _requestModal.GenerateRequestModal();
    }
    
    public void GenerateTaskList()
    {
        StartCoroutine(GenerateUI());
    }

    public VisualElement GetRootComponent(){return _root;}
    public VisualElement GetCardContainer() {return _requestCardContainer;}
}
