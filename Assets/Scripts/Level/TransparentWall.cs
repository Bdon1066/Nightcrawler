using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransparentWall : MonoBehaviour
{
    [Header("Material Refrences")]
    public Material OpaqueMaterial;
    public Material transparentMaterial;
    private bool playerSignal;
    private bool opaque;
    public void SignalTransparent()
    {
        playerSignal = true;
    }
    private void Update()
    {
        if (!playerSignal)
        {
            opaque = true;
        }
        if (opaque && playerSignal)
        {
            ChangeMaterial();
            opaque = false;
        }
        if (opaque)
        {
            Opaque();
        }
        playerSignal = false;
       
    }
    public void Opaque()
    {
        GetComponent<MeshRenderer>().material = OpaqueMaterial;
    }
    void ChangeMaterial()
    {
        GetComponent<MeshRenderer>().material = transparentMaterial;
    }
}



