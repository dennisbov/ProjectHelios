using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerPhisicsMovement))]
[RequireComponent(typeof(TestDoorPlacement))]
public class PlayerController : MonoBehaviour
{
    private PlayerPhisicsMovement _playerMovement;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _doorOpeningDistance;
    [SerializeField] private LayerMask _pickableLayer;
    [SerializeField] private LayerMask _glassLayer;
    [SerializeField] private float JumpForce;
    public Transform PickableObjectTransform;

    private bool _isHandsFull = false;
    private Pickable _currentPickable;

    public TestDoorPlacement DoorPlacement { get; private set; }

    private void Start()
    {
        DoorPlacement = GetComponent<TestDoorPlacement>();
        _playerMovement = GetComponent<PlayerPhisicsMovement>();
    }
    private float Hdir;
    private float Vdir;

    private bool _wasJustPlaced = false;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _doorOpeningDistance, _pickableLayer))
            {
                if(hit.collider.TryGetComponent(out Door door))
                {
                    door.DoorPair.SwitchDoorStates(door);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (_isHandsFull)
            {
                if (_currentPickable.Place())
                {
                    _currentPickable = null;
                    _isHandsFull = false;
                    _wasJustPlaced = true;
                }
            }
            if (_wasJustPlaced == false && Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _doorOpeningDistance, _pickableLayer))
            {
                if (hit.collider.TryGetComponent(out Pickable item))
                {
                    if (_isHandsFull == false)
                    {
                        _currentPickable = item;
                        item.Pickup(this);
                        _isHandsFull = true;
                    }
                }
            }
            _wasJustPlaced = false;
        }
    }

    public void OpenNaNoor()
    {
        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _doorOpeningDistance, _pickableLayer))
        {
            if (hit.collider.TryGetComponent(out Door door))
            {
                if (door.DoorPair.IsOpen == false)
                {
                    door.DoorPair.OpenDoors(door, true);
                }
            }
        }
    }

    public void BreakGlass()
    {
        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _doorOpeningDistance, _glassLayer))
        {
            if (hit.collider.TryGetComponent(out GlassWall glass))
            {
                glass.Break();
            }
        }
    }

    public void ClearOffHands()
    {
        if(_currentPickable != null)
        {
            if (_currentPickable.Place())
            {
                _currentPickable = null;
                _isHandsFull = false;
                _wasJustPlaced = true;
            }
        }
    }
    private void HandleBaseMovement()
    {
        //Hdir = Input.GetAxisRaw("Horizontal");
        //Vdir = Input.GetAxisRaw("Vertical");
        //_playerMovement.Move(new Vector3(Hdir, Vdir));

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    _playerMovement.AddImpulse(Vector3.up * JumpForce);
        //}
    }
}
