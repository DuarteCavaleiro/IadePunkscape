using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Mannequins {
    public class CheckCompletionRate : MonoBehaviour {
        [SerializeField] XRSocketInteractor xrWristSocket;
        // Start is called before the first frame update
        [SerializeField] private float threshold;
        private Dictionary<string, float> StylePoints = new() {
            {"Punk", 0},
            {"Goth", 0},
            {"Metal", 0}
        };
        private bool ReachedThreshold = false;
        private string ThresholdReached = "none";
        void Start() {
            xrWristSocket.selectEntered.AddListener(OnObjectPlaced);
            xrWristSocket.selectExited.AddListener(OnObjectRemoved);
        }
    
    
        private void OnObjectPlaced(SelectEnterEventArgs args)  {
            XRBaseInteractable interactable = args.interactableObject  as XRBaseInteractable;
            if (interactable != null) {
                ArticleTraits script = interactable.GetComponent<ArticleTraits>();
                if (script != null) {
                    var articlePoints = script.getStylepoints();
                    foreach (var kvp in articlePoints) {
                        if (StylePoints.ContainsKey(kvp.Key)) {
                            StylePoints[kvp.Key] += kvp.Value; 
                        } else {
                            StylePoints[kvp.Key] = kvp.Value;   
                        }
                    }
                    CheckThreshold();
                } else {
                    Debug.LogWarning("No script found on placed object!");
                }
            }
        }
    
        private void OnObjectRemoved(SelectExitEventArgs arg0)  {
            XRBaseInteractable interactable = arg0.interactableObject  as XRBaseInteractable;
            if (interactable != null) {
                ArticleTraits script = interactable.GetComponent<ArticleTraits>();
                if (script != null) {
                    var articlePoints = script.getStylepoints();
                    foreach (var kvp in articlePoints) {
                        if (StylePoints.ContainsKey(kvp.Key)) {
                            StylePoints[kvp.Key] -= kvp.Value; 
                        } else {
                            StylePoints[kvp.Key] = kvp.Value; //figure out if it should equal to 0 or be the same
                        }
                    }
                    CheckThreshold();
                } else {
                    Debug.LogWarning("No script found on placed object!");
                }
            }
        }

        private void CheckThreshold() {
            foreach (var kvp in StylePoints) {
                Debug.Log(kvp.Key + " : " + kvp.Value);
                if (kvp.Value >= threshold) {
                    ReachedThreshold = true;
                    ThresholdReached = kvp.Key;
                    Debug.Log("ThresholdReached with style" + ThresholdReached);
                }
            }
        }
    }
}
