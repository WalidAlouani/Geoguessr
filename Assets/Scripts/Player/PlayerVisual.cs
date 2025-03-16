using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private PlayerSetupByTurn setup;

    public void Init(int i)
    {
        _renderer.material = setup.entries[i].material;
    }
}
