using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    protected PlayerController Player;
    protected bool IsPicked = false;
    [SerializeField] protected LayerMask InHandLayer;
    [SerializeField] private MeshRenderer[] _renderers;
    private LayerMask realLayer;

    public virtual void Pickup(PlayerController player) 
    { 
        Player = player;
        realLayer = gameObject.layer;
        SetLayers(LayerMask.NameToLayer("InHand"));
        gameObject.layer = LayerMask.NameToLayer("InHand");
    }

    public virtual bool Place()
    {
        gameObject.layer = realLayer.value;
        SetLayers(realLayer.value);
        return true;
    }

    private void SetLayers(LayerMask layer)
    {
        if (_renderers != null)
        {
            foreach (var renderer in _renderers)
            {
                renderer.gameObject.layer = layer;
            }
        }
    }
}
