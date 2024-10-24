using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SheetManager : MonoBehaviour
{
    [Header("API")]
    private string _baseURL = "https://sheets.googleapis.com/v4/spreadsheets/";
    private string _property = "/values/";
    private string _keyString = "?key=";
    private string _tabName_Dev = "Dev";
    private string _tabName_Prod = "Today";
    
    private string _sheetUrl;
    private bool _isDev = true; //switch between dev and prod
    
    //dev
    private string _apiKey = "AIzaSyCCzE8MUPDIQPFovwiYAgmaZBtA5Y1_lHs";
    private string _sheetId = "16ZNq8X-tG6_l7dOviIyPOnpbjfgaqsFocbO5aRvzLPo";
 
    [Header("UI")] 
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private RequestManager _requestManager;
    
    [Header("Request")] 
    private Dictionary<string, Request> _requestDict = new Dictionary<string, Request>();
    private Dictionary<string, Request> _archivedRequestDict = new Dictionary<string, Request>();
    [SerializeField] private PendingRequestsTab _pendingRequestsTab;
    [SerializeField] private ArchivedRequestsTab _archivedRequestsTab;
    [SerializeField] private RequestModal _requestModal;
    [SerializeField] private List<Request> _requestList= new List<Request>(); //for debug purposes
    [SerializeField] private List<Request> _archivedRequestList= new List<Request>(); //debug

    [Header("Internet Connectivity")] 
    [SerializeField] private Connectivity _connectivity;
    

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _requestUI.GenerateUI();
    }

    private void Start()
    {
        _sheetUrl = _isDev 
            ? _baseURL + _sheetId + _property + _tabName_Dev + _keyString + _apiKey 
            : _baseURL + _sheetId + _property + _tabName_Prod + _keyString + _apiKey;
        
        Debug.Log(_sheetUrl);

        InvokeRepeating("ObtainSheetData", 0f, 2f);
        InvokeRepeating("UpdatePendingRequestCountUI", 0f, 0.5f);
        InvokeRepeating("UpdateArchivedRequestCountUI", 0f, 0.5f);
    }

    void ObtainSheetData()
    {
        StartCoroutine(ObtainSheetDataRoutine());
    }

    IEnumerator ObtainSheetDataRoutine()
    {
        if (!_connectivity.IsConnectedToInternet())
            SceneManager.LoadScene("Login");
        
        UnityWebRequest webRequest = UnityWebRequest.Get(_sheetUrl);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
            ProcessData(webRequest.downloadHandler.text);
        else
            Debug.LogError("Error: " + webRequest.error);
    }
    
    void ProcessData(string json)
    {
        var obj = JSON.Parse(json);
        var arr = obj["values"].AsArray;
        for (int i = 0; i < arr.Count; i++)
        {
            var item = arr[i];
            if (arr.Count < 2)
            {
                if (_archivedRequestDict.Count != 0)
                    _requestManager.RemoveAllArchivedRequests(_archivedRequestDict, _archivedRequestsTab, _archivedRequestList);
                if (_requestDict.Count != 0)
                    _requestManager.RemoveAllPendingRequests(_requestDict, _pendingRequestsTab, _requestList);
            }
            else if(item[6] == "Houseperson")
            {
                Request request = TransformRequestData (item);
                UpsertRequestData(request);
                if (_requestModal.IsModalOpen() && _requestModal.GetCurrentRequestID() == request.id)
                {
                    _requestModal.GenerateModalContent(request);
                    _requestModal.UpdateModalUI(request);
                }
            }
        }
    }

    Request TransformRequestData (JSONNode item)
    {
        Request request = new Request
        {
            id = item[0],
            timeReceived = item[1],
            area = item[2],
            guestName = item[3],
            details = item[4],
            type = item[5],
            receiver = item[6],
            priority = item[7],
            submitter = item[8],
            status = item[9],
            notes = "",
            timeCompleted = "",
            handler = "",
            isViewed =  item[13]
        };
        return request;
    }

    void UpsertRequestData(Request request)
    {
        if (!_requestDict.ContainsKey(request.id) && request.status == "On-going")
            _requestManager.AddRequest(request, _requestDict, _requestList);
        else
            HandleExistingRequest(request);
            
    }

    private void HandleExistingRequest(Request request)
    {
        if (request.status == "Complete")
        {
            _requestManager.RemoveRequest(request, _requestDict, _pendingRequestsTab, _requestList);
            _requestManager.AddArchivedRequest(request, _archivedRequestDict, _archivedRequestList);
        }
        else if (request.status == "Aborted")
        {
            _requestManager.RemoveRequest(request, _requestDict, _pendingRequestsTab, _requestList);
        }
        else if (request.status == "On-going")
        {
            _requestDict[request.id] = request;
            _requestManager.UpdateRequestCard(request, _pendingRequestsTab, _requestList);
            
            if(_archivedRequestDict.ContainsKey(request.id))
                _requestManager.RemoveArchivedRequest(request, _archivedRequestDict, _archivedRequestsTab, _archivedRequestList);
        }
    }

    private void UpdatePendingRequestCountUI(){ _pendingRequestsTab.UpdatePendingRequestCountUI(_requestDict.Count);}
    private void UpdateArchivedRequestCountUI(){ _archivedRequestsTab.UpdateArchivedRequestCountUI(_archivedRequestDict.Count);}
    public bool IsDev(){ return _isDev;}
}

