using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] private Animator _connectedDoor;
    [SerializeField] private Mesh _pressedCondition;
    [SerializeField] private Mesh _releasedCondition;
    [SerializeField] private float _absenceCooldown;

    private MeshFilter _currentMesh;

    private void Awake()
    {
        _currentMesh = GetComponentInChildren<MeshFilter>();
    }

    private float _expiredTime = 0;
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody interactable))
        {
            _connectedDoor.SetBool("IsOpen", true);
            _currentMesh.mesh = _pressedCondition;
            _expiredTime = 0;
        }
    }

    private void Update()
    {
        if(_expiredTime > _absenceCooldown)
        {
            _connectedDoor.SetBool("IsOpen", false);
            _currentMesh.mesh = _releasedCondition;
        }
        _expiredTime += Time.deltaTime;
    }

}
