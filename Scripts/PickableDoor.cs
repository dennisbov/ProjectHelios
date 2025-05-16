using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Door))]
public class PickableDoor : Pickable
{
    private Door _door;
    private void Awake()
    {
        _door = GetComponent<Door>();
    }
    public override bool Place()
    {
        if (Player.DoorPlacement.Door.activeSelf == false)
        {
            return false;
        }
        _door.PlaceDoor(Player.DoorPlacement.Door.transform.position, Player.DoorPlacement.Door.transform.rotation);
        Player.DoorPlacement.enabled = false;
        IsPicked = false;
        _door.DoorPair.ToggleCollider(true);
        base.Place();
        return true;
    }

    private void Update()
    {
        if( IsPicked ) 
        {
            _door.PlaceDoor(Player.PickableObjectTransform.position, Player.PickableObjectTransform.rotation);
        }
    }

    public override void Pickup(PlayerController player)
    {
        base.Pickup(player);
        _door.DoorPair.CloseDoors();
        player.DoorPlacement.enabled = true;
        player.DoorPlacement.Door = _door.DoorPair.TransparentDoor;
        _door.DoorPair.ToggleCollider(false);
        IsPicked = true;
    }

}
