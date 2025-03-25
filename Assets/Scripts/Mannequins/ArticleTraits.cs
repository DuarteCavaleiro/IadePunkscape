using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticleTraits : MonoBehaviour
{
    [Serializable] 
    public class KeyValuePairData {
        public string key;
        public float value;
    }
    [SerializeField] private List<KeyValuePairData> serializedDictionary = new();
    private Dictionary<string, float> stylePoints= new();
    // Start is called before the first frame update
    void Start() {
        foreach (var item in serializedDictionary) {
            stylePoints[item.key] = item.value;
        }
    }

    public Dictionary<string, float> getStylepoints(){
        return stylePoints;
    }
}
