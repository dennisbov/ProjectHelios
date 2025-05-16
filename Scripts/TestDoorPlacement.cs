using UnityEngine;
using System.Collections.Generic;
using System;

public class TestDoorPlacement : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _testTransform;
    [SerializeField] public GameObject Door;
    [SerializeField] private float _placementRange;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _wallDistance;
    [Range(0,1)]
    [SerializeField] private float _yNormalOffset;

    [NonSerialized] public Quaternion PortalRotation;

    private BoxCollider _doorCollider;
    private void OnEnable()
    {
        _doorCollider = Door.GetComponent<BoxCollider>();
        Door.SetActive(false);
    }

    private void OnDisable()
    {
        Door.SetActive(false);
    }
    private void Update()
    {
        if(Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _placementRange, _wallLayer))
        {
            PlaceDoor(hit.point, CalculateDoorRotation(hit), hit);
            Door.transform.forward = hit.normal;
        }
        else
        {
            Door.SetActive(false);
        }
        
    }

    private void PlaceDoor(Vector3 position, Quaternion rotation, RaycastHit initialHit)
    {
        _testTransform.position = position;
        _testTransform.rotation = rotation;
        _testTransform.position -= _testTransform.forward * 0.001f;

        FixOverhangs();
        FixIntersects();

        bool _isOnRightWall = Physics.Raycast(_testTransform.position, Vector3.down, out RaycastHit hit, 1000, _groundLayer);

        if (CheckOverlap() && _isOnRightWall && Mathf.Abs(initialHit.normal.y) < _yNormalOffset)
        {
            Door.transform.position = hit.point + Vector3.up * _doorCollider.size.y / 2 + Door.transform.forward * _wallDistance;
            PortalRotation = CalculateDoorRotation(hit);
            Door.SetActive(true);
        }
        else
        {
            Door.SetActive(false);
        }
    }

    private void FixOverhangs()
    {
        var testPoints = new List<Vector3>
        {
            new Vector3(-1.1f,  0.0f, 0.1f),
            new Vector3( 1.1f,  0.0f, 0.1f),
            new Vector3( 0.0f, -2.1f, 0.1f),
            new Vector3( 0.0f,  2.1f, 0.1f)
        };

        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = _testTransform.TransformPoint(testPoints[i]);
            Vector3 raycastDir = _testTransform.TransformDirection(testDirs[i]);

            if (Physics.CheckSphere(raycastPos, 0.05f, _wallLayer))
            {
                break;
            }
            else if (Physics.Raycast(raycastPos, raycastDir, out hit, 2.1f, _wallLayer))
            {
                var offset = hit.point - raycastPos;
                _testTransform.Translate(offset, Space.World);
            }
        }
    }

    private bool CheckOverlap()
    {
        var checkExtents = new Vector3(0.9f, 1.9f, 0.05f);

        var checkPositions = new Vector3[]
        {
            _testTransform.position + _testTransform.TransformVector(new Vector3( 0.0f,  0.0f, -0.1f)),

            _testTransform.position + _testTransform.TransformVector(new Vector3(-1.0f, -2.0f, -0.1f)),
            _testTransform.position + _testTransform.TransformVector(new Vector3(-1.0f,  2.0f, -0.1f)),
            _testTransform.position + _testTransform.TransformVector(new Vector3( 1.0f, -2.0f, -0.1f)),
            _testTransform.position + _testTransform.TransformVector(new Vector3( 1.0f,  2.0f, -0.1f)),

            _testTransform.TransformVector(new Vector3(0.0f, 0.0f, 0.2f))
        };

        // Ensure the portal does not intersect walls.
        var intersections = Physics.OverlapBox(checkPositions[0], checkExtents, _testTransform.rotation, _wallLayer);

        if (intersections.Length > 1)
        {
            return false;
        }
        else if (intersections.Length == 1)
        {
            // We are allowed to intersect the old portal position.
            if (intersections[0] != _doorCollider)
            {
                return false;
            }
        }

        // Ensure the portal corners overlap a surface.
        bool isOverlapping = true;

        for (int i = 1; i < checkPositions.Length - 1; ++i)
        {
            isOverlapping &= Physics.Linecast(checkPositions[i],
                checkPositions[i] + checkPositions[checkPositions.Length - 1], _wallLayer);
        }

        return isOverlapping;
    }

    private void FixIntersects()
    {
        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        var testDists = new List<float> { 1.1f, 1.1f, 2.1f, 2.1f };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = _testTransform.TransformPoint(0.0f, 0.0f, -0.1f);
            Vector3 raycastDir = _testTransform.TransformDirection(testDirs[i]);

            if (Physics.Raycast(raycastPos, raycastDir, out hit, testDists[i], _wallLayer))
            {
                var offset = (hit.point - raycastPos);
                var newOffset = -raycastDir * (testDists[i] - offset.magnitude);
                _testTransform.Translate(newOffset, Space.World);
            }
        }
    }

    private Quaternion CalculateDoorRotation(RaycastHit hit)
    {
        var cameraRotation = _camera.rotation;
        var portalRight = cameraRotation * Vector3.right;

        if (Mathf.Abs(portalRight.x) >= Mathf.Abs(portalRight.z))
        {
            portalRight = (portalRight.x >= 0) ? Vector3.right : -Vector3.right;
        }
        else
        {
            portalRight = (portalRight.z >= 0) ? Vector3.forward : -Vector3.forward;
        }

        var portalForward = -hit.normal;
        var portalUp = Vector3.Cross(portalRight, portalForward);

        var portalRotation = Quaternion.LookRotation(portalForward, portalUp);
        return portalRotation;
    }
}
