using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortalInteraction : PortalableObject
{
    private PlayerRotation cameraMove;

    protected override void Awake()
    {
        base.Awake();

        cameraMove = GetComponent<PlayerRotation>();
    }

    public override void Warp()
    {
        base.Warp();
        cameraMove.ResetTargetRotation();
    }
}
