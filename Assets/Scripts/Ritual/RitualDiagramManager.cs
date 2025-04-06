using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class RitualDiagramManager : MonoBehaviour
{
   [SerializeField] private List<GameObject> Arrows;
   [SerializeField] private GameObject Circle;
   

   private void CreateDiagram()
   {
      for (int i = 0; i < Arrows.Count; i++)
      {
         float angle = i * (360f / Arrows.Count);
         
         Arrows[i].transform.rotation = Quaternion.Euler(0, angle, 0);
         Arrows[i].GetComponent<RitualArrowManager>().SetIndexNumber(i);
      }
   }

   private void CheckIfArrowIsOn()
   {
      for (int i = 0; i < Arrows.Count; i++)
      {
         if (Arrows[i].GetComponent<RitualArrowManager>().IsActive) Arrows.RemoveAt(i);
      }
      ActivateDiagramCircle();
   }

   private void ActivateDiagramCircle()
   {
      if (Arrows.Count == 0)
      {
         Circle.SetActive(true);
      }
   }

   private void Start()
   {
      CreateDiagram();
   }

   private void OnEnable()
   {
      EventManager.ArrowActivatedEvent.AddListener(CheckIfArrowIsOn);
   }

   private void OnDisable()
   {
      EventManager.ArrowActivatedEvent.RemoveListener(CheckIfArrowIsOn);
   }
}