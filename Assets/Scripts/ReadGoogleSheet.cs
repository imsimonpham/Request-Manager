using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

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
    
    [SerializeField] private List<Request> _requestList= new List<Request>(); //for debug purposes
    private Dictionary<string, Request> _requestDictionary = new Dictionary<string, Request>();

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
        UnityWebRequest www = UnityWebRequest.Get(_url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            var obj = JSON.Parse(json);

            if (obj["values"].IsArray)
            {
                var arr = obj["values"].AsArray;
                for (int i = 0; i < arr.Count; i++)
                {
                    var item = arr[i];
                    if (i > 0 && item[9] == "Sent")
                    {
                        Request request = new Request();
                        request.id = item[0];
                        request.timeReceived = item[1];
                        request.area = item[2];
                        request.guestName = item[3];
                        request.details = item[4];
                        request.type = item[5];
                        request.receiver = item[6];
                        request.priority = item[7];
                        request.submitter = item[8];
                        request.isSent = item[9];
                        request.status = item[10];
                        request.resolutionDetails = "";
                        request.timeCompleted = "";
                        request.handler = "";

                        if (!_requestDictionary.ContainsKey(request.id))
                        {
                            _requestList.Add(request); //debug purposes
                            _requestDictionary.Add(request.id, request);
                            _requestkListUI.CreateSingleRequest(request.timeReceived, request.area, request.details, request.priority);
                        }
                        else
                        {
                            _requestDictionary[request.id] = request;
                            var existingRequest = _requestList.Find(r => r.id == request.id);
                            if(existingRequest != null)
                            {
                                int index = _requestList.IndexOf(existingRequest);
                                _requestList[index] = request;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Error: " + www.error);
        }
    }
    
    void UpdateRequestist()
    {
        foreach (KeyValuePair<string, Request> kvp in _requestDictionary)
        {
            
        }
    }
}

[Serializable]
public class Request
{
    public string id;
    public string timeReceived;
    public string area;
    public string guestName;
    public string details;
    public string type;
    public string receiver;
    public string priority;
    public string submitter;
    public string isSent;
    public string status;
    public string resolutionDetails;
    public string timeCompleted;
    public string handler;
}