using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowActivatorManager : MonoBehaviour
{
    [SerializeField] private RitualArrowManager AssignedArrow;
   
    public void ActivatedAssignedArrow()
    {
        AssignedArrow.SetArrowOn();
        EventManager.ArrowActivatedEvent.Invoke();
    }
}
