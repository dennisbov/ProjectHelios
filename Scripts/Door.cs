using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour
{

    public DoorPair DoorPair;

    public Quaternion PortalRotation { private set; get; }
    public Transform Visual;

    public Transform PortalPosition;
    private Animator _animator;
    private Collider _collider;

    [NonSerialized]public bool IsPortalRemoved = true;

    [SerializeField] private AudioClip _doorOpen;
    [SerializeField] private AudioClip _doorSlam;
    [SerializeField] private AudioClip _doorClose;
    [SerializeField] private AudioSource _doorOpenAudio;

    private void Awake()
    {
        PortalRotation = PortalPosition.rotation;
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
    }

    public void Open(bool _isSlamed)
    {
        _animator.SetBool("Open", true);
        IsPortalRemoved = true;
        if (_isSlamed)
        {
            _doorOpenAudio.PlayOneShot(_doorSlam);
        }
        else
        {
            _doorOpenAudio.PlayOneShot(_doorOpen);
        }
    }

    public void Close() 
    {
        _animator.SetBool("Open", false);
    }

    public void removePortals()
    {
        if (IsPortalRemoved)
        {
            DoorPair.RemovePortals();
            DoorPair.ResetScale();
        }
    }

    public void ToggleCollider(bool value)
    {
        _collider.enabled = value;
    }

    public void PlaceDoor(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        PortalRotation = PortalPosition.rotation;
    }

    public void CloseOpenDoorSound()
    {
        _doorOpenAudio.PlayOneShot(_doorClose);
    }
}
