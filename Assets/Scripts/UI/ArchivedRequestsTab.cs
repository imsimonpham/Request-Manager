using UnityEngine;
using UnityEngine.UIElements;

public class ArchivedRequestsTab : MonoBehaviour
{
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private UIUtilities _uiUtilities;
    private VisualElement _archiveRequestsTab;
    private ScrollView _cardContainer;
    private VisualElement _bottomIcon;
    private Label _tabTitle;
    private string _tabTitleText = "Archived Requests";
    
    
    public void GenerateArchivedRequestsTab()
    {
        //create tab
        _archiveRequestsTab = _uiUtilities.CreateAndAddToParent<VisualElement>("tab", _requestUI.GetContainer());
        _tabTitle = _uiUtilities.CreateAndAddToParent<Label>("h2", _archiveRequestsTab);
        _uiUtilities.UpdateLabel(_tabTitle, _tabTitleText, "tabTitle");
        _archiveRequestsTab.AddToClassList("hide");
        
        //create card container
        _cardContainer = new ScrollView(ScrollViewMode.Vertical);
        _cardContainer.AddToClassList("cardContainer");
        _cardContainer.touchScrollBehavior = ScrollView.TouchScrollBehavior.Clamped;
        _requestUI.GetContainer().Add(_cardContainer);
    }
    
    public VisualElement GetArchiveRequestsTab() {return _archiveRequestsTab;}
    public VisualElement GetCardContainer() {return _cardContainer;}
}
