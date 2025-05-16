using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathColllider : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerController player))
        {
            player.transform.position = _startPosition.position;
        }
    }
}
