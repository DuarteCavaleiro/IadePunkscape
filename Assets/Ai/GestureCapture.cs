using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GestureCapture : MonoBehaviour
{
    public string currentLabel = "square";  // Update via UI if needed
    private List<Vector2> currentGesture = new List<Vector2>();
    private List<GestureSample> allGestures = new List<GestureSample>();
    private bool isDrawing = false;

    private LineRenderer lineRenderer;
    public Material lineMaterial;
    public float lineWidth = 0.05f;

    void Start()
    {
        // Create the line renderer
        GameObject lineObj = new GameObject("GestureLine");
        lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial != null ? lineMaterial : new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentGesture.Clear();
            isDrawing = true;
            lineRenderer.positionCount = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
            if (currentGesture.Count > 5)
            {
                allGestures.Add(new GestureSample
                {
                    label = currentLabel,
                    points = currentGesture.ToArray()
                });
                Debug.Log($"Gesture '{currentLabel}' saved. Total: {allGestures.Count}");
            }
        }

        if (isDrawing)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentGesture.Add(mousePos);
            UpdateLineRenderer();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SaveGestures();
        }
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = currentGesture.Count;
        for (int i = 0; i < currentGesture.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(currentGesture[i].x, currentGesture[i].y, 0));
        }
    }

    void SaveGestures()
    {
        string json = JsonHelper.ToJson(allGestures.ToArray(), true);
        File.WriteAllText(Application.dataPath + "/gestures.json", json);
        Debug.Log("Gesture data saved to gestures.json");
    }

    [System.Serializable]
    public class GestureSample
    {
        public string label;
        public Vector2[] points;
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T> { Items = array };
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
