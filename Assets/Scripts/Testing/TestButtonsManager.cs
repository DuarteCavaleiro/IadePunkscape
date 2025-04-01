using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestButtonsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Arrows;
    [SerializeField] private TextMeshProUGUI ButtonText;

    public int _index;

    private GameObject _affectedArrow;

    public void Cycle(int increment)
    {
        _index += increment;
        
        _index = (_index + Arrows.Count) % Arrows.Count;

        _affectedArrow = Arrows[_index];

        ButtonText.text = "Arrow " + (_index + 1);
    }

    public void ActivateArrow()
    {
        _affectedArrow.GetComponent<RitualArrowManager>().SetArrowOn();
        EventManager.ArrowActivatedEvent.Invoke();
    }

    private void Start()
    {
        _index = 0;
        ButtonText.text = "Arrow " + (_index + 1);
        _affectedArrow = Arrows[_index];
    }
}
