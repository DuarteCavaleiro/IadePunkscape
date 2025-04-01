using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualArrowManager : MonoBehaviour
{
    [SerializeField] private Texture OffTexture;
    [SerializeField] private Texture OnTexture;
    [SerializeField] private GameObject ParticleSystem;
    [SerializeField] private GameObject Quad;

    private Renderer _rend;

    public bool IsActive;
    private void Start()
    {
        _rend = Quad.GetComponent<Renderer>();
        //SetArrowOn();
    }

    public void SetArrowOn()
    {
        _rend.material.mainTexture = OnTexture;
        //ParticleSystem.SetActive(true);
        IsActive = true;
    }
}
