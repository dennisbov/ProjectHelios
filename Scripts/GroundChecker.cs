using UnityEngine;

public class GroundChecker : MonoBehaviour
{

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    public enum groundState
    {
        onGround,
        offGround
    };

    public groundState getGroundState()
    {
        if (Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer))
            return groundState.onGround;
        else
            return groundState.offGround;
    }
}

