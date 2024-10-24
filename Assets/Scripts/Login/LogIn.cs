using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.SceneManagement;

public class LogIn : MonoBehaviour
{
    [SerializeField] private UIDocument _doc;
    [SerializeField] private StyleSheet _styleSheet;
    [SerializeField] private UIUtilities _uiUtilities;
    [SerializeField] private Connectivity _connectivity;
    private VisualElement _root;
    private VisualElement _container;

    private void Awake()
    {
        StartCoroutine(GenerateUIRoutine());
    }

    private void OnValidate()
    {
        if (Application.isPlaying) return;
        StartCoroutine(GenerateUIRoutine());
    }
    
    private IEnumerator GenerateUIRoutine()
    {
        yield return null;
        _root = _doc.rootVisualElement;
        _root.Clear();
        _root.styleSheets.Add(_styleSheet);

        //container
        _container = _uiUtilities.CreateAndAddToParent<VisualElement>("container login", _root);
        
        //app title
        var title = _uiUtilities.CreateAndAddToParent<Label>("h1 upperCenter title", _container);
        _uiUtilities.UpdateLabel(title, "Request Manager", "title");
        
        //body
        var subContainer = _uiUtilities.CreateAndAddToParent<VisualElement>("subContainer", _container);
        
        //Connectivity warning text
        var connectivityText = _uiUtilities.CreateAndAddToParent<Label>("h4 warning", subContainer);
        _uiUtilities.UpdateLabel(connectivityText, "Please connect to the Internet!", "message");
        
        //Connectivity warning icon
        var iconConatiner = _uiUtilities.CreateAndAddToParent<VisualElement>("icon singleIcon", subContainer);
        var noWifiIcon = _uiUtilities.CreateAndAddToParent<Image>("noWifi", iconConatiner);
    }
    
    private void Update()
    {
        if (_connectivity.IsConnectedToInternet())
            SceneManager.LoadScene("Main");
    }
}
