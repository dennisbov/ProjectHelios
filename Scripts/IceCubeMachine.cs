using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCubeMachine : MonoBehaviour
{
    [SerializeField] private GameObject _iceCubePref;
    [SerializeField] private Transform _spawnPosition;

    private void Start()
    {
        SpawnIceCube();
    }

    private void SpawnIceCube()
    {
        GameObject ice = Instantiate(_iceCubePref, _spawnPosition.position, _spawnPosition.rotation);
        ice.GetComponent<Destroyable>().OnDestroy.AddListener(SpawnIceCube);
    }
}
