using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ReadGooleSheet : MonoBehaviour
{
    [Header("API")]
    public string baseURL = "https://sheets.googleapis.com/v4/spreadsheets/";
    public string property = "/values/";
    public string keyString = "?key=";
    public string tabName = "Today";
    public string apiKey = "AIzaSyCCzE8MUPDIQPFovwiYAgmaZBtA5Y1_lHs";
    public string sheetId = "16ZNq8X-tG6_l7dOviIyPOnpbjfgaqsFocbO5aRvzLPo";
    public string url;

    [Header("UI")] 
    public TaskListUI _taskList;

    [Header("Request Details")] 
    public string  area;
    public string  timeReceived;
    public string  guestName;
    public string  request;
    public string  priority;
    

    private void Start()
    {
        url = baseURL + sheetId + property + tabName + keyString + apiKey;
        InvokeRepeating("ObtainSheetDataRoutine", 0f, 1f);
    }

    void ObtainSheetDataRoutine()
    {
        StartCoroutine(ObtainSheetData());
    }

    IEnumerator ObtainSheetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            var obj = JSON.Parse(json);

            if (obj["values"].IsArray)
            {
                var arr = obj["values"].AsArray;
                /*Debug.Log(arr[0]);*/
                for (int i = 0; i < arr.Count; i++)
                {
                    var request = arr[i];
                    if (i > 0)
                    {
                        Debug.Log(request);
                        /*Debug.Log(item[2]);*/
                        /*area.text = "#" + item[2];
                        timeReceived.text = item[1];
                        request.text = item[4];
                        priority.text = item[6];*/
                        _taskList.GenerateTaskList(
                            request[2], 
                            request[1], 
                            request[4], 
                            request[6],
                            request[9]
                        );
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Error: " + www.error);
        }
    }
}
