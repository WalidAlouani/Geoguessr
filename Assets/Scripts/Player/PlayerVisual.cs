using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Material[] materials;

    public void SetVisual(int i)
    {
        _renderer.material = materials[i];
    }
}
