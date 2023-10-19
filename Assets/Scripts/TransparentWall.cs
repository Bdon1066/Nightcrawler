using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWall : MonoBehaviour
{
    [Header("Material Refrences")]
    public Material OpaqueMaterial;
    public Material transparentMaterial;
    private bool shouldBeTransparent = false;

    public void Transparent()
    {
        this.GetComponent<MeshRenderer>().material = transparentMaterial;
        shouldBeTransparent = true;
    }
    private void Update()
    {
        shouldBeTransparent = false;
        if (!shouldBeTransparent)
        {
            Opaque();
        }
    }
    public void Opaque()
    {
        this.GetComponent<MeshRenderer>().material = OpaqueMaterial;
    }

}
