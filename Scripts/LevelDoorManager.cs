using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDoorManager : MonoBehaviour
{
    [SerializeField] private DoorPair[] doors;

    private void Awake()
    {
        foreach (DoorPair doorPair in doors) 
        {
            doorPair.DoorManager = this;
        }    
    }
    public void CloseOtherDoors(DoorPair ignored)
    {
        foreach(DoorPair doorPair in doors) 
        { 
            if(doorPair != ignored)
            {
                doorPair.CloseDoorsWithoutRemovingPortals();
            }
        }
    }
}
