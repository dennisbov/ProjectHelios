using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerPhisicsMovement))]
public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private float mouseSpeed;
    [SerializeField] private Transform camera;
    [SerializeField] private float maxCamRotation;
    [SerializeField] private float minCamRotation;
    [SerializeField] private float smoothTime;
    [SerializeField] private GroundChecker GroundChecker;
    [SerializeField] private float _sprintingRotateSlowdownModifier = 0.2f;

    private PlayerPhisicsMovement _playerPhisicsMovement;

    private Rigidbody player;
    private Quaternion playerTargetRot;
    private Quaternion cameraTargetRot;
    private bool _isRotatable = true;

    // Start is called before the first frame update
    void Start()
    {
        playerTargetRot = transform.localRotation;
        cameraTargetRot = camera.localRotation;
        _playerPhisicsMovement = GetComponent<PlayerPhisicsMovement>();
    }

    // Update is called once per frame

    private float mouseMovementX;
    private float mouseMovementY;

    private void Update()
    {
        if(_isRotatable == false)
        {
            return;
        }
        float totalMouseSpeed;
        if (_playerPhisicsMovement.IsSprinting)
        {
            totalMouseSpeed = mouseSpeed * _sprintingRotateSlowdownModifier;
        }
        else
        {
            totalMouseSpeed = mouseSpeed;
        }
        mouseMovementX = Input.GetAxis("Mouse X") * totalMouseSpeed;
        mouseMovementY = Input.GetAxis("Mouse Y") * totalMouseSpeed;

        playerTargetRot *= Quaternion.Euler(0, mouseMovementX, 0);
        cameraTargetRot *= Quaternion.Euler(-mouseMovementY, 0, 0);

        cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot, minCamRotation, maxCamRotation);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, playerTargetRot, smoothTime * Time.deltaTime);
        camera.localRotation = Quaternion.Slerp(camera.localRotation, cameraTargetRot, smoothTime * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.H))
        {
            Cursor.visible = !Cursor.visible;
        }
    }
    private Quaternion ClampRotationAroundXAxis(Quaternion q, float MinimumX, float MaximumX)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    public void ResetTargetRotation()
    {
        playerTargetRot = Quaternion.LookRotation(transform.forward, Vector3.up);
    }

    public IEnumerator FallingIntoShaft(Quaternion taretRotation, float speed)
    {
        _isRotatable = false;
        while (camera.rotation != taretRotation) {
            transform.rotation = Quaternion.Euler(0, Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, taretRotation.eulerAngles.y, speed * Time.deltaTime), 0);
            camera.localRotation = Quaternion.Euler(Mathf.MoveTowardsAngle(camera.localRotation.eulerAngles.x, taretRotation.eulerAngles.x, speed * Time.deltaTime), 0, 0); 
            yield return null;
        }
    }
}
