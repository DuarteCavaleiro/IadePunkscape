using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GestureSender : MonoBehaviour
{
    public string serverUrl = "http://localhost:5000/predict";
    private List<Vector2> drawnPoints = new List<Vector2>();
    private bool isDrawing = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            drawnPoints.Clear();
            isDrawing = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
            StartCoroutine(SendGestureToServer());
        }

        if (isDrawing)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            drawnPoints.Add(mousePos);
        }
    }

    IEnumerator SendGestureToServer()
    {
        if (drawnPoints.Count < 5)
        {
            Debug.Log("Not enough points to classify gesture.");
            yield break;
        }

        // Prepare JSON array of {"x":..., "y":...}
        List<Dictionary<string, float>> pointList = new List<Dictionary<string, float>>();
        foreach (var point in drawnPoints)
        {
            pointList.Add(new Dictionary<string, float> {
                { "x", point.x },
                { "y", point.y }
            });
        }

        Dictionary<string, object> payload = new Dictionary<string, object>
        {
            { "points", pointList }
        };

        string jsonData = JsonUtility.ToJson(new Wrapper { points = pointList });

        UnityWebRequest request = new UnityWebRequest(serverUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Server responded: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    [System.Serializable]
    public class Wrapper
    {
        public List<Dictionary<string, float>> points;
    }
}
