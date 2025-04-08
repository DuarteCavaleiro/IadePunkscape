using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Mannequins {
    public class CheckCompletionRate : MonoBehaviour {
        [SerializeField] List<XRSocketInteractor> interactors = new();
        [SerializeField] List<XRGrabInteractable> clothesOnMannequin = new();
        // Start is called before the first frame update
        [SerializeField] private float threshold;
        private Dictionary<string, float> StylePoints = new() {
            {"Punk", 0},
            {"Goth", 0},
            {"Metal", 0}
        };

        private bool ReachedThreshold;
        [SerializeField] private string Style = "style";
        [SerializeField] private GameObject floorSign;
        [SerializeField] private GameObject foreheadPattern;
        [SerializeField] private GameObject handRecognition;
        [SerializeField] internal GameObject handGestureLeft;
        [SerializeField] internal GameObject handGestureRight;

        [SerializeField] private GameObject LeftEyeCenter;
        [SerializeField] private GameObject RightEyeCenter;
        [SerializeField] private GameObject LeftEyeTrail;
        [SerializeField] private GameObject RightEyeTrail;
        
        [SerializeField] internal bool inInteractionRange;
        [SerializeField] internal bool recording;

        void Start() {
            foreach (var interactor in interactors)
            {
                interactor.selectEntered.AddListener(OnObjectPlaced);
                interactor.selectExited.AddListener(OnObjectRemoved);
            }
            
            switch (Style)
            {
                case "Punk":
                    LeftEyeCenter.GetComponent<ParticleSystemRenderer>().material.color = Color.red;
                    LeftEyeCenter.GetComponent<ParticleSystemRenderer>().material.color = Color.red;
                    RightEyeCenter.GetComponent<ParticleSystemRenderer>().material.color = Color.red;
                    LeftEyeTrail.GetComponent<ParticleSystemRenderer>().material.color = Color.red;
                    RightEyeCenter.GetComponent<ParticleSystemRenderer>().material.color = Color.red;
                    LeftEyeCenter.GetComponent<Light>().color = Color.red;
                    RightEyeCenter.GetComponent<Light>().color = Color.red;
                    break;
                case "Goth":
                    Color purple = new Color(0.6f, 0.2f, 0.8f, 1f);
                    LeftEyeCenter.GetComponent<ParticleSystemRenderer>().material.color = purple;
                    RightEyeCenter.GetComponent<ParticleSystemRenderer>().material.color = purple;
                    LeftEyeTrail.GetComponent<ParticleSystemRenderer>().material.color = purple;
                    RightEyeCenter.GetComponent<ParticleSystemRenderer>().material.color = purple;
                    LeftEyeCenter.GetComponent<Light>().color = purple;
                    RightEyeCenter.GetComponent<Light>().color = purple;
                    break;
                
                case "Metal":
                    LeftEyeCenter.GetComponent<ParticleSystemRenderer>().material.color = Color.white;
                    RightEyeCenter.GetComponent<ParticleSystemRenderer>().material.color = Color.white;
                    LeftEyeTrail.GetComponent<ParticleSystemRenderer>().material.color = Color.white;
                    RightEyeCenter.GetComponent<ParticleSystemRenderer>().material.color = Color.white;
                    LeftEyeCenter.GetComponent<Light>().color = Color.white;
                    RightEyeCenter.GetComponent<Light>().color = Color.white;
                    break;
                
                default:
                    Debug.Log("No Style Set");
                    break;
            }
        }
    
    
        private void OnObjectPlaced(SelectEnterEventArgs args)  {
            XRBaseInteractable interactable = args.interactableObject  as XRBaseInteractable;
            if (interactable != null) {
                ArticleTraits script = interactable.GetComponent<ArticleTraits>();
                clothesOnMannequin.Add(interactable.gameObject.GetComponent<XRGrabInteractable>());
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
                ArticleTraits script = interactable.gameObject.GetComponent<ArticleTraits>();
                clothesOnMannequin.Remove(interactable.gameObject.GetComponent<XRGrabInteractable>()); 
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
            ReachedThreshold = false;
            floorSign.SetActive(false);
            foreheadPattern.SetActive(false);

            var leftCenterEmission = LeftEyeCenter.GetComponent<ParticleSystem>().emission;
            var rightCenterEmission = RightEyeCenter.GetComponent<ParticleSystem>().emission;
            
            foreach (var kvp in StylePoints) {
                Debug.Log(kvp.Key + " : " + kvp.Value);
                if(kvp.Key == Style){
                    
                    float normalized = Mathf.InverseLerp(0f, 100f, kvp.Value);
                    float emissionRate = normalized * 20;
                    leftCenterEmission.rateOverTime = emissionRate;
                    rightCenterEmission.rateOverTime = emissionRate;
                    
                    if (kvp.Value >= threshold) {
                        ReachedThreshold = true;
                        floorSign.SetActive(true);
                        foreheadPattern.SetActive(true);

                        LeftEyeCenter.GetComponent<Light>().enabled = true;
                        RightEyeCenter.GetComponent<Light>().enabled = true;
                        
                        Debug.Log("ThresholdReached with style" + Style);
                    }        
                }
            }
        }


        public void StartRecording() {
            if (ReachedThreshold & inInteractionRange) {
                recording = true;
                Debug.Log("Recording");
            }
        }
        public void StopRecording() {
            if (ReachedThreshold & inInteractionRange) {
                recording = false;
                Debug.Log("StoppedRecording");
                if (true) 
                {
                    foreach (var clothe in clothesOnMannequin) 
                    { 
                        var currentLayers = clothe.interactionLayers;
                        clothe.interactionLayers = currentLayers & ~InteractionLayerMask.GetMask("Default"); 
                        handRecognition.SetActive(false);
                    }
                    LeftEyeTrail.SetActive(true);
                    RightEyeCenter.SetActive(true);
                }
            }
        }
    }
}
