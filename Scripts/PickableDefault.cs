using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PickableDefault : Pickable
{

    private Rigidbody _rigidbody;
    private Collider _collider;
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    public override bool Place()
    {
        base.Place();
        IsPicked = false;
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
        if (Player.TryGetComponent(out DestructablePickablesManager manager))
        {
            manager.Item = null;
        }
        return true;
    }
    private void Update()
    {
        if (IsPicked)
        {
            transform.position = Player.PickableObjectTransform.position;
            transform.rotation = Player.PickableObjectTransform.rotation;
        }
    }

    public override void Pickup(PlayerController player)
    {
        base.Pickup(player);
        IsPicked=true;
        _collider.enabled=false;
        _rigidbody.isKinematic=true;
        if(gameObject.TryGetComponent(out Destroyable destroyable) && Player.TryGetComponent(out DestructablePickablesManager manager))
        {
            manager.Item = destroyable;
        }
    }

}
