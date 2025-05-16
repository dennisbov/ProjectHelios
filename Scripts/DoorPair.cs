using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPair : MonoBehaviour
{
    [SerializeField] private Door _door1;
    [SerializeField] private Door _door2;
    
    [SerializeField] private PortalPair _portalPair;

    [NonSerialized] public LevelDoorManager DoorManager;
    public GameObject TransparentDoor;
    public bool IsOpen = false;

    public void SwitchDoorStates(Door invoker)
    {
        if (IsOpen == false)
        {
            OpenDoors(invoker, false);
        }
        else
        {
            CloseDoors();
        }

    }
    public void OpenDoors(Door invoker, bool _isSlamed)
    {
        _door1.Visual.localScale = new Vector3(-1, 1, -1);
        _door2.Visual.localScale = new Vector3(-1, 1, -1);
        invoker.Visual.localScale = Vector3.one;

        DoorManager.CloseOtherDoors(this);

        _door1.Open(_isSlamed);
        _portalPair.Portals[0].PlacePortal(_door1.PortalPosition.position, _door1.PortalRotation);
        _door2.Open(_isSlamed);
        _portalPair.Portals[1].PlacePortal(_door2.PortalPosition.position, _door2.PortalRotation);
        IsOpen = true;
    }

    public void CloseDoors()
    {
        _door1.Close();
        _door2.Close();
        IsOpen = false;
    }

    public void CloseDoorsWithoutRemovingPortals()
    {
        _door1.IsPortalRemoved = false;
        _door2.IsPortalRemoved = false;
        CloseDoors();
        ResetScale();
    }

    public void ResetScale()
    {
        _door1.Visual.localScale = Vector3.one;
        _door2.Visual.localScale = Vector3.one;
    }

    public void RemovePortals()
    {
        _portalPair.Portals[0].RemovePortal();
        _portalPair.Portals[1].RemovePortal();
    }

    public void ToggleCollider(bool value)
    {
        _door1?.ToggleCollider(value);
        _door2?.ToggleCollider(value);
    }
}

