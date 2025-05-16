using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerController))]
public class PlayerPhisicsMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSprintingSpeed;
    [SerializeField] private float _timeToAccelerate;
    [SerializeField] private GameObject _speedParticles;
    [SerializeField] private float _maxFallingSpeed;
    [SerializeField] private AudioSource _rollingSound;
    [SerializeField] private float _rollingSoundModifier;
    [SerializeField] private float _rollingSoundPitchModifier;
    public bool IsSprinting { get; private set; }
    public bool IsOnMaxSpeed;


    private Rigidbody _rb;
    private PlayerController _playerController;
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _rb.velocity = Vector3.down * _maxFallingSpeed;
    }

    private float _horizontalMovement;
    private float _verticalMovement;
    private Vector3 _inputVelocity;
    private float _expiredTime;
    [NonSerialized] public bool _isMovable = true;

    void FixedUpdate()
    {
        if (_rb.velocity.y < -_maxFallingSpeed)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, -_maxFallingSpeed, _rb.velocity.z);
        }
        if (_isMovable == false)
        {
            return;
        }
        _horizontalMovement = Input.GetAxis("Horizontal");

        if (IsSprinting)
        {
            _verticalMovement = _expiredTime / _timeToAccelerate * _maxSprintingSpeed + _speed;
            _inputVelocity = (transform.forward * _verticalMovement + transform.right * _horizontalMovement * _speed) * Time.fixedDeltaTime;
            if(_expiredTime < _timeToAccelerate)
            {
                _expiredTime += Time.fixedDeltaTime;
            }
            else
            {
                _playerController.BreakGlass();
                _expiredTime = _timeToAccelerate;
                _speedParticles.SetActive(true);
                IsOnMaxSpeed = true;
            }
            _playerController.OpenNaNoor();
        }
        else
        {
            IsOnMaxSpeed = false;
            _speedParticles.SetActive(false);
            _expiredTime = 0;
            _verticalMovement = Input.GetAxis("Vertical");
            Vector3 normalizedFrameVelocity = transform.forward * _verticalMovement + transform.right * _horizontalMovement;
            if(normalizedFrameVelocity.magnitude > 1)
                normalizedFrameVelocity.Normalize();
            _inputVelocity = normalizedFrameVelocity * _speed * Time.fixedDeltaTime;
        }
        _rollingSound.volume = (Mathf.Abs(_horizontalMovement) + Mathf.Abs(_verticalMovement)) * _rollingSoundModifier;
        _rollingSound.pitch = Mathf.Clamp(_rb.velocity.magnitude * _rollingSoundPitchModifier, 1, 3);
        _rb.velocity = new Vector3(_inputVelocity.x, _rb.velocity.y, _inputVelocity.z);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _rb.velocity.magnitude >= _inputVelocity.magnitude*0.8f)
        {
            IsSprinting = true;
        }
        else
        {
            IsSprinting = false;
        }
    }
    public IEnumerator FallingIntoShaft(Vector3 fallingPosition, float adjustedSpeed, float minDistance)
    {
        _rb.velocity = new Vector3(0, _rb.velocity.y, 0);

        _isMovable = false;

        while (MathF.Sqrt(MathF.Pow(transform.position.x - fallingPosition.x, 2) + MathF.Pow(transform.position.z - fallingPosition.z, 2)) > minDistance)
        {
            transform.position = new Vector3(
                    Mathf.Lerp(transform.position.x, fallingPosition.x, adjustedSpeed * Time.deltaTime),
                    transform.position.y,
                    Mathf.Lerp(transform.position.z, fallingPosition.z, adjustedSpeed * Time.deltaTime)
                );
            yield return null;
        }
    }
}
