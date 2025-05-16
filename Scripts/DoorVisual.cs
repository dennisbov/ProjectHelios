using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorVisual : MonoBehaviour
{
    [SerializeField] private DoorVisual _connectedDoor;
    [SerializeField] Camera _outDoorView;

    private void Start()
    {
        _connectedDoor._outDoorView.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        GetComponentInChildren<MeshRenderer>().sharedMaterial.mainTexture = _connectedDoor._outDoorView.targetTexture;
    }

    private void Update()
    {
        // Position
        Vector3 lookerPosition = _connectedDoor.transform.worldToLocalMatrix.MultiplyPoint3x4(Camera.main.transform.position);
        lookerPosition = new Vector3(-lookerPosition.x, lookerPosition.y, -lookerPosition.z);
        _outDoorView.transform.localPosition = lookerPosition;

        // Rotation
        Quaternion difference = transform.rotation * Quaternion.Inverse(_connectedDoor.transform.rotation * Quaternion.Euler(0, 180, 0));
        _outDoorView.transform.rotation = difference * Camera.main.transform.rotation;

        // Clipping
        _outDoorView.nearClipPlane = lookerPosition.magnitude;
    }
}
