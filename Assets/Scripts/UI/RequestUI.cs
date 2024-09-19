using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class RequestUI : MonoBehaviour
{
    [SerializeField] private UIDocument _doc;
    [SerializeField] private StyleSheet _styleSheet;
    [SerializeField] private UIUtilities _uiUtilities;
    [SerializeField] private PendingRequestsTab _pendingRequestsComponent;
    [SerializeField] private ArchivedRequestsTab _archivedRequestsComponent;
    [SerializeField] private NotesTab _notesComponent;
    [SerializeField] private RequestModal _requestModal;
    private VisualElement _pendingTasksIconContainer;
    private VisualElement _archivedTasksIconContainer;
    private VisualElement _notesIconContainer;
    private List<VisualElement> _iconContainerList = new List<VisualElement>();
    private VisualElement _root;
    private VisualElement _container;
    private VisualElement _bottomMenu;
    /*private string _tabTitleText;*/
    
    //App hierarchy
    /*
        * RequestUI (root)
            * container  
                * PendingRequestTab
                    * RequestCard     
                * ArchiveListTab
                * NoteTab
                * ChecklistTab
            * BottomMenu
            * RequestModal (position: absolute)
     */
    
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        StartCoroutine(GenerateUIRoutine());
    }

    public IEnumerator GenerateUIRoutine()
    
    {
        yield return null;
        _root = _doc.rootVisualElement;
        _root.Clear();
        _root.styleSheets.Add(_styleSheet);

        //container
        _container = _uiUtilities.CreateAndAddToParent<VisualElement>("container", _root);
        
        //app title
        var title = _uiUtilities.CreateAndAddToParent<Label>("h1 upperCenter title", _container);
        _uiUtilities.UpdateLabel(title, _uiUtilities.GetToday(), "title");
        
        _pendingRequestsComponent.GeneratePendingRequestsTab();
        _archivedRequestsComponent.GenerateArchivedRequestsTab();
        _notesComponent.GenerateNotesTab();
        GenerateBottomMenu();
        
        _requestModal.GenerateRequestModal();
    }

    private void GenerateBottomMenu()
    {
        _bottomMenu = _uiUtilities.CreateAndAddToParent<VisualElement>("bottomMenu", _root);
        _pendingTasksIconContainer = _uiUtilities.CreateAndAddToParent<VisualElement>("bottomIcon", _bottomMenu);
        _archivedTasksIconContainer = _uiUtilities.CreateAndAddToParent<VisualElement>("bottomIcon", _bottomMenu);
        _notesIconContainer = _uiUtilities.CreateAndAddToParent<VisualElement>("bottomIcon", _bottomMenu);
        _pendingTasksIconContainer.name = "pendingTaskIconContainer";
        _archivedTasksIconContainer.name = "archivedTasksIconContainer";
        _notesIconContainer.name = "notesIconContainer";
        _iconContainerList.Add(_pendingTasksIconContainer);
        _iconContainerList.Add(_archivedTasksIconContainer);
        _iconContainerList.Add(_notesIconContainer);
        
        _uiUtilities.CreateAndAddToParent<Image>("pendingTask", _pendingTasksIconContainer);
        _uiUtilities.CreateAndAddToParent<Image>("archive", _archivedTasksIconContainer);
        _uiUtilities.CreateAndAddToParent<Image>("notes", _notesIconContainer);
        
        GenerateActiveTab(_archivedTasksIconContainer);
        _pendingTasksIconContainer.RegisterCallback<ClickEvent>(evt => GenerateActiveTab(_pendingTasksIconContainer));
        _archivedTasksIconContainer.RegisterCallback<ClickEvent>(evt => GenerateActiveTab(_archivedTasksIconContainer));
        _notesIconContainer.RegisterCallback<ClickEvent>(evt => GenerateActiveTab(_notesIconContainer));
    }

    private void GenerateActiveTab(VisualElement iconContainer)
    {
        if (iconContainer.Q<VisualElement>(name: "pendingTaskIconContainer") != null)
        {
            _pendingRequestsComponent.GetPendingRequestsTab().RemoveFromClassList("hide"); // scroll view
            _archivedRequestsComponent.GetArchiveRequestsTab().AddToClassList("hide"); // scroll view
            _notesComponent.GetNotesTab().AddToClassList("archivedTasksIconContainer"); // scroll view
            SetActiveBorder(_pendingTasksIconContainer);
        }
        else if (iconContainer.Q<VisualElement>(name: "archivedTasksIconContainer") != null)
        {
            _pendingRequestsComponent.GetPendingRequestsTab().AddToClassList("hide");
            _archivedRequestsComponent.GetArchiveRequestsTab().RemoveFromClassList("hide");
            _notesComponent.GetNotesTab().AddToClassList("hide");
            SetActiveBorder(_archivedTasksIconContainer);
        }
        else if (iconContainer.Q<VisualElement>(name: "notesIconContainer") != null)
        {
            _pendingRequestsComponent.GetPendingRequestsTab().AddToClassList("hide");
            _archivedRequestsComponent.GetArchiveRequestsTab().AddToClassList("hide"); 
            _notesComponent.GetNotesTab().RemoveFromClassList("hide");
            SetActiveBorder(_notesIconContainer);
        }
    }

    private void SetActiveBorder(VisualElement iconContainer)
    {
        for(var i = 0; i < _iconContainerList.Count; i++)
        {
            _iconContainerList[i].RemoveFromClassList("active");
            if (_iconContainerList[i].name == iconContainer.name)
                _iconContainerList[i].AddToClassList("active");
        }
    }
    
    public void GenerateUI() {StartCoroutine(GenerateUIRoutine());}
    public VisualElement GetRootComponent(){return _root;}
    public VisualElement GetContainer(){return _container;}
}
