using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class SheetManager : MonoBehaviour
{
    [Header("API")]
    private string _baseURL = "https://sheets.googleapis.com/v4/spreadsheets/";
    private string _property = "/values/";
    private string _keyString = "?key=";
    private string _tabName = "Today";
    private string _apiKey = "AIzaSyCCzE8MUPDIQPFovwiYAgmaZBtA5Y1_lHs";
    private string _sheetId = "16ZNq8X-tG6_l7dOviIyPOnpbjfgaqsFocbO5aRvzLPo";
    private string _sheetUrl;
    /*[SerializeField] private int _apiCallCount;*/
    private string _formUrl = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfsKNFIN2RQIx37zLb0Sj2ynBhfyWqB0Z2zrJhJco6B40wjbw/formResponse";
 
    [Header("UI")] 
    [SerializeField] private RequestUI _requestUI;
    [SerializeField] private RequestManager _requestManager;
    
    [Header("Request Data")] 
    private Dictionary<string, Request> _requestDict = new Dictionary<string, Request>();
    private Dictionary<string, Request> _archivedRequestDict = new Dictionary<string, Request>();
    [SerializeField] private PendingRequestsTab _pendingRequestsTab;
    [SerializeField] private ArchivedRequestsTab _archivedRequestsTab;
    [SerializeField] private List<Request> _requestList= new List<Request>(); //for debug purposes
    [SerializeField] private List<Request> _archivedRequestList= new List<Request>(); //debug
    

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _requestUI.GenerateUI();
    }

    private void Start()
    {
        _sheetUrl = _baseURL + _sheetId + _property + _tabName + _keyString + _apiKey;
        InvokeRepeating("ObtainSheetData", 0f, 1f);
    }

    void ObtainSheetData()
    {
        StartCoroutine(ObtainSheetDataRoutine());
    }

    IEnumerator ObtainSheetDataRoutine()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(_sheetUrl);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
            ProcessData(webRequest.downloadHandler.text);
        else
            Debug.LogError("Error: " + webRequest.error);
        
        /*_apiCallCount++;*/
    }
    
    void ProcessData(string json)
    {
        var obj = JSON.Parse(json);
        var arr = obj["values"].AsArray;
        for (int i = 0; i < arr.Count; i++)
        {
            var item = arr[i];
            if (i > 0)
            {
                Request request = TransformRequestData (item);
                UpsertRequestData(request);
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
            resolution = "",
            timeCompleted = "",
            handler = ""
        };
        return request;
    }

    void UpsertRequestData(Request request)
    {
        if (!_requestDict.ContainsKey(request.id) && request.status == "On-going")
        {
            _requestManager.AddRequest(request, _requestDict, _requestList);
        }
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
}

