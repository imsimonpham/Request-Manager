using UnityEngine;
using UnityEngine.UIElements;

public class NotesTab : MonoBehaviour
{
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private UIUtilities _uiUtilities;
    private VisualElement _notesTab;
    private ScrollView _textContainer;
    private VisualElement _bottomIcon;
    private Label _tabTitle;
    private string _tabTitleText = "Today's note";
    
    
    public void GenerateNotesTab()
    {
        //create tab
        _notesTab = _uiUtilities.CreateAndAddToParent<VisualElement>("tab", _requestUI.GetContainer());
        _tabTitle = _uiUtilities.CreateAndAddToParent<Label>("h2", _notesTab);
        _uiUtilities.UpdateLabel(_tabTitle, _tabTitleText, "tabTitle");
        _notesTab.AddToClassList("hide");
        
        //create text container
        _textContainer = new ScrollView(ScrollViewMode.Vertical);
        _textContainer.AddToClassList("cardContainer");
        _textContainer.touchScrollBehavior = ScrollView.TouchScrollBehavior.Clamped;
        _requestUI.GetContainer().Add(_textContainer);
    }
    
    public VisualElement GetNotesTab() {return _notesTab;}
    public VisualElement GetTextContainer() {return _textContainer;}
}