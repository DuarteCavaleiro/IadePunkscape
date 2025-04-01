using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowActivatorManager : MonoBehaviour
{
    [SerializeField] private RitualArrowManager AssignedArrow;

    private void ActivatedAssignedArrow()
    {
        AssignedArrow.SetArrowOn();
    }
}
