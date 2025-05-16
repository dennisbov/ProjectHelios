using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TestDoorPlacement))]
public class DoorInteractions : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _doorOpeningDistance;
    [SerializeField] private LayerMask _doorLayer;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private Transform _doorInHandPosition;

    private Door _doorInHands;
    private TestDoorPlacement _doorPlacement;
    private bool _isHandsFull = false;

    private void Awake()
    {
        _doorPlacement = GetComponent<TestDoorPlacement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _doorOpeningDistance, _doorLayer))
            {
                if (hit.collider.TryGetComponent(out Door door))
                {
                    door.DoorPair.SwitchDoorStates(door);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _doorOpeningDistance, _doorLayer))
            {
                if (hit.collider.TryGetComponent(out Door door) && _isHandsFull == false)
                {
                    door.Close();
                    door.ToggleCollider(false);
                    door.transform.position = _doorInHandPosition.position;
                    door.transform.rotation = _doorInHandPosition.rotation;
                    _doorInHands = door;
                    _doorPlacement.enabled = true;
                    _isHandsFull = true;
                } 
            }
            if(Physics.Raycast(_camera.position, _camera.forward, out RaycastHit wallHit, _doorOpeningDistance, _wallLayer))
            {
                if (_isHandsFull == true && _doorPlacement.Door.activeInHierarchy)
                {
                    _doorInHands.PlaceDoor(_doorPlacement.Door.transform.position,
                        _doorPlacement.Door.transform.rotation
                        );
                    _doorInHands.ToggleCollider(true);
                    _doorPlacement.enabled = false;
                    _isHandsFull = false;
                    _doorInHands = null;
                }
            }
        }
    }
}
