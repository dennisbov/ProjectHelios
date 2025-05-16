using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerPhisicsMovement))]
public class DestructablePickablesManager : MonoBehaviour
{
    [NonSerialized] public Destroyable Item;
    [SerializeField] private LayerMask _hotAreaLayer;
    [SerializeField] private string _hotAreaTag = "HotArea";

    private PlayerPhisicsMovement _playerPhisicsMovement;
    private PlayerController _playerController;
    private void Awake()
    {
        _playerPhisicsMovement = GetComponent<PlayerPhisicsMovement>();
        _playerController = GetComponent<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(_hotAreaTag) && Item != null && _playerPhisicsMovement.IsOnMaxSpeed == false)
        {   
            Item.Destroy();
            _playerController.ClearOffHands();
            Item = null;
        }
    }
}
