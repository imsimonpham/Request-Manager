using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class ReadGoogleSheet : MonoBehaviour
{
    [Header("API")]
    private string _baseURL = "https://sheets.googleapis.com/v4/spreadsheets/";
    private string _property = "/values/";
    private string _keyString = "?key=";
    private string _tabName = "Today";
    private string _apiKey = "AIzaSyCCzE8MUPDIQPFovwiYAgmaZBtA5Y1_lHs";
    private string _sheetId = "16ZNq8X-tG6_l7dOviIyPOnpbjfgaqsFocbO5aRvzLPo";
    private string _url;

    [Header("UI")] 
    [SerializeField] private RequestListUI _requestkListUI;
    private Dictionary<string, VisualElement> _requestUIDict = new Dictionary<string, VisualElement>();
    
    [Header("Request Data")] 
    [SerializeField] private List<Request> _requestList= new List<Request>(); //for debug purposes
    private Dictionary<string, Request> _requestDict = new Dictionary<string, Request>();

    private void Awake()
    {
        _requestkListUI.GenerateTaskList();
    }

    private void Start()
    {
        _url = _baseURL + _sheetId + _property + _tabName + _keyString + _apiKey;
        InvokeRepeating("ObtainSheetDataRoutine", 0f, 1f);
    }

    void ObtainSheetDataRoutine()
    {
        StartCoroutine(ObtainSheetData());
    }

    IEnumerator ObtainSheetData()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(_url);
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
            if (i > 0 && item[9] == "Sent")
            {
                Request request = CreateRequest(item);
                UpsertRequestData(request);
                UpsertRequestCard(request);
            }
        }
    }

    Request CreateRequest(JSONNode item)
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
            isSent = item[9],
            status = item[10],
            resolutionDetails = "",
            timeCompleted = "",
            handler = ""
        };
        return request;
    }

    void UpsertRequestData(Request request)
    {
        if (!_requestDict.ContainsKey(request.id)) 
        {
            _requestList.Add(request); //debug
            _requestDict.Add(request.id, request); 
        }
        else 
        {
            _requestDict[request.id] = request;
            //debug
            var existingRequest = _requestList.Find(r => r.id == request.id);
            if(existingRequest != null)
            {
                int index = _requestList.IndexOf(existingRequest);
                _requestList[index] = request;
            }
        }
    }
    
    void UpsertRequestCard(Request request)
    {
        if (!_requestUIDict.ContainsKey(request.id))
        {
            VisualElement requestCard = _requestkListUI.CreateRequestCard(request.timeReceived, request.area, request.details, request.priority);
            _requestUIDict.Add(request.id, requestCard);
        }
        else
        {
            _requestkListUI.UpdateRequestCard(_requestUIDict[request.id], request.area, request.details, request.priority);
        }
    }
}

