using UnityEngine;
using UnityEngine.UIElements;

public class PendingRequestsTab : MonoBehaviour
{
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private UIUtilities _uiUtilities;
    private VisualElement _pendingRequestsTab;
    private ScrollView _cardContainer;
    private VisualElement _bottomIcon;
    private Label _tabTitle;
    private string _tabTitleText = "Pending Requests";

    public void GeneratePendingRequestsTab()
    {
        //create tab
        _pendingRequestsTab = _uiUtilities.CreateAndAddToParent<VisualElement>("tab", _requestUI.GetContainer());
        _tabTitle = _uiUtilities.CreateAndAddToParent<Label>("h2", _pendingRequestsTab);
        _uiUtilities.UpdateLabel(_tabTitle, _tabTitleText, "tabTitle");
        _pendingRequestsTab.AddToClassList("hide");

        //create card container
        _cardContainer = new ScrollView(ScrollViewMode.Vertical);
        _cardContainer.AddToClassList("cardContainer");
        _cardContainer.touchScrollBehavior = ScrollView.TouchScrollBehavior.Clamped;
        _pendingRequestsTab.Add(_cardContainer);
    }

    public VisualElement GetPendingRequestsTab(){ return _pendingRequestsTab;}
    public VisualElement GetCardContainer(){return _cardContainer;}
}
