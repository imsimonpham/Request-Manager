using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class RequestUI : MonoBehaviour
{
    [SerializeField] private UIDocument _doc;
    [SerializeField] private StyleSheet _styleSheet;
    [SerializeField] private UIUtilities _uiUtilities;
    [SerializeField] private PendingRequestsTab _pendingRequestsComponent;
    [SerializeField] private ArchivedRequestsTab _archivedRequestsComponent;
    [SerializeField] private NotesTab _notesComponent;
    [SerializeField] private RequestModal _requestModal;
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
        var iconContainer_1 = _uiUtilities.CreateAndAddToParent<VisualElement>("bottomIcon", _bottomMenu);
        var iconContainer_2 = _uiUtilities.CreateAndAddToParent<VisualElement>("bottomIcon", _bottomMenu);
        var iconContainer_3 = _uiUtilities.CreateAndAddToParent<VisualElement>("bottomIcon", _bottomMenu);
        
        var pendingTasksIcon = _uiUtilities.CreateAndAddToParent<Image>("pendingTask", iconContainer_1);
        var archiveIcon = _uiUtilities.CreateAndAddToParent<Image>("archive", iconContainer_2);
        var notesIcon = _uiUtilities.CreateAndAddToParent<Image>("notes", iconContainer_3);
        
        GenerateActiveTab(archiveIcon);
        pendingTasksIcon.RegisterCallback<ClickEvent>(evt => GenerateActiveTab(pendingTasksIcon));
        archiveIcon.RegisterCallback<ClickEvent>(evt => GenerateActiveTab(archiveIcon));
        notesIcon.RegisterCallback<ClickEvent>(evt => GenerateActiveTab(notesIcon));
    }

    private void GenerateActiveTab(VisualElement icon)
    {
        if (icon.Q<VisualElement>(className: "pendingTask") != null)
        {
            _pendingRequestsComponent.GetPendingRequestsTab().RemoveFromClassList("hide"); // scroll view
            _archivedRequestsComponent.GetArchiveRequestsTab().AddToClassList("hide"); // scroll view
            _notesComponent.GetNotesTab().AddToClassList("hide"); // scroll view
        }
        else if (icon.Q<VisualElement>(className: "archive") != null)
        {
            _pendingRequestsComponent.GetPendingRequestsTab().AddToClassList("hide");
            _archivedRequestsComponent.GetArchiveRequestsTab().RemoveFromClassList("hide");
            _notesComponent.GetNotesTab().AddToClassList("hide");
        }
        else if (icon.Q<VisualElement>(className: "notes") != null)
        {
            _pendingRequestsComponent.GetPendingRequestsTab().AddToClassList("hide");
            _archivedRequestsComponent.GetArchiveRequestsTab().AddToClassList("hide"); 
            _notesComponent.GetNotesTab().RemoveFromClassList("hide");
        }
    }
    
    public void GenerateUI() {StartCoroutine(GenerateUIRoutine());}
    public VisualElement GetRootComponent(){return _root;}
    public VisualElement GetContainer(){return _container;}
}
