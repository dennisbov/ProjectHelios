using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SceneChanger))]
[RequireComponent(typeof(Collider))]
public class LiftShaft : MonoBehaviour
{
    private Collider _collider;
    [SerializeField] private Transform _playerPosition;

    [SerializeField] private float _adjusmentSpeed;
    [SerializeField] private float _minDistance;
    [SerializeField] private Transform _repetablePosition;
    [SerializeField] private float _timeToSceneChange;

    private SceneChanger _changer;
    private bool _isFalling;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _changer = GetComponent<SceneChanger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerPhisicsMovement player))
        {
            if (_isFalling == false)
            {
                StartCoroutine(player.FallingIntoShaft(_playerPosition.position, _adjusmentSpeed, _minDistance));
                StartCoroutine(player.gameObject.GetComponent<PlayerRotation>().FallingIntoShaft(_playerPosition.rotation, _adjusmentSpeed));
                _isFalling = true;
                _expiredTime = 0;
            }
            else
            {
                player.transform.position = _repetablePosition.position;
            }
        }
        
    }

    private float _expiredTime;
    private void Update()
    {

        if (_isFalling && _expiredTime > _timeToSceneChange) 
        {
            _changer.ChangeScene();
        }
        _expiredTime += Time.deltaTime;
    }
}
