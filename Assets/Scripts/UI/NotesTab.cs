using UnityEngine;
using UnityEngine.UIElements;

public class NotesTab : MonoBehaviour
{
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private UIUtilities _uiUtilities;
    
    private VisualElement _notesTab;
    private VisualElement _textContainer;
    private Label _tabTitle;
    private string _tabTitleText = "Today's notes";
    private TextField _notesInput;
    
    public void GenerateNotesTab()
    {
        //create tab
        _notesTab = _uiUtilities.CreateAndAddToParent<VisualElement>("tab", _requestUI.GetContainer());
        _tabTitle = _uiUtilities.CreateAndAddToParent<Label>("h2", _notesTab);
        _uiUtilities.UpdateLabel(_tabTitle, _tabTitleText, "tabTitle");
        _notesTab.AddToClassList("hide");
        
        //create text container
        _textContainer = _uiUtilities.CreateAndAddToParent<VisualElement>("textContainer", _notesTab);
        
        //note field
        _notesInput = _uiUtilities.CreateAndAddToParent<TextField>("notesTextField margin_bottom_lg", _textContainer);
        _notesInput.multiline = true;
        
        LoadNote();
        InvokeRepeating("SaveNote", 0, 0.1f);
    }

    private void SaveNote()
    {
        PlayerPrefs.SetString("SavedNote", _notesInput.text);
        PlayerPrefs.Save();
    }
    
    private void LoadNote()
    {
        if (PlayerPrefs.HasKey("SavedNote"))
            _notesInput.value = PlayerPrefs.GetString("SavedNote");
    }
    
    public VisualElement GetNotesTab() {return _notesTab;}
}