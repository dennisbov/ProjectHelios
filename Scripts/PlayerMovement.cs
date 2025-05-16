using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [Header("On ground settings")]
    [SerializeField] private float _groundInputAcceleration;
    [SerializeField] private float _groundFrictionInfluence;
    [Header("Off ground settings")]
    [SerializeField] private float _offGroundInputAcceleration;
    [SerializeField] private float _offGroundAirFrictionInfluence;

    [Header("otherSettings")]
    [SerializeField] private float _freeFallAcceleration;
    [SerializeField] private bool _includeFriction;
    [SerializeField] private float _reflectionCooldown;
    [SerializeField] private TrajectoryReflectionType _tragectoryReflectionMode;
    [SerializeField] private float _sinWaveSquizing = 1;
    [Range(0, 1)]
    [SerializeField] private float _planeProjectingPower;
    [SerializeField] private float _maxGravitationVectorMagnitude;

    private CharacterController _player;
    private float _currentInputAcceleration;
    private float _currentFrictionInfluence;
    private float _expiredCollisionTime;
    private Vector3 _startPosition;
    private bool _isMovementAvaliable;

    [SerializeField] private GroundChecker groundChecker;
    [SerializeField] private GameObject _camera;
    [Header("Test settings")]
    [SerializeField] private bool _isPlayerTestEntity;

    private const float _teleportingDuration = 0.1f;

    private void Awake()
    {
        _player = GetComponent<CharacterController>();
        _currentFrictionInfluence = _groundFrictionInfluence;
        _currentInputAcceleration = _groundInputAcceleration;
        _startPosition = transform.position;
        _isMovementAvaliable = _isPlayerTestEntity;
        _camera.SetActive(_isPlayerTestEntity);
    }

    private Vector3 _totalMovement;
    private Vector3 _inputMovementVector;
    private Vector3 _gravityInfluenceVector;
    //private Vector3 impulseMotionVector;
    private Vector3 _externalForces;

    private void FixedUpdate()
    {
        if (_isMovementAvaliable == false)
            return;

        AdjustGroundDependendSettings();

        SetInputMovementVector(ref _inputMovementVector);

        CalculateCurrentGravityVector(ref _gravityInfluenceVector);

        if (_includeFriction)
            AdjustFriction(ref _externalForces);

        _externalForces += _gravityInfluenceVector;

        _totalMovement = _inputMovementVector + _externalForces;

        _player.Move(_totalMovement * Time.fixedDeltaTime);

        _expiredCollisionTime += Time.fixedDeltaTime;
    }

    private Vector2 _moveDirection;
    public void Move(Vector2 direction)
    {
        _moveDirection = direction.normalized;
    }

    private void SetInputMovementVector(ref Vector3 inputVector)
    {
        inputVector = transform.TransformDirection(new Vector3(_moveDirection.x, 0, _moveDirection.y)) * _currentInputAcceleration;
    }

    private void AdjustFriction(ref Vector3 currentMotionVector)
    {
        float frictionForce = _currentFrictionInfluence;
        currentMotionVector += -currentMotionVector * frictionForce;
    }

    private void AdjustGroundDependendSettings()
    {
        if (groundChecker.getGroundState() == GroundChecker.groundState.onGround)
        {
            _currentFrictionInfluence = _groundFrictionInfluence;
            _currentInputAcceleration = _groundInputAcceleration;
        }
        else if (groundChecker.getGroundState() == GroundChecker.groundState.offGround)
        {
            _currentFrictionInfluence = _offGroundAirFrictionInfluence;
            _currentInputAcceleration = _offGroundInputAcceleration;
        }
    }

    public void AddImpulse(Vector3 force)
    {
        _externalForces *= 0.1f;
        _externalForces += force;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_expiredCollisionTime > _reflectionCooldown)
        {
            switch (_tragectoryReflectionMode)
            {
                case (TrajectoryReflectionType.bounce):
                    ReflectTrajectory(hit.normal, ref _externalForces);
                    break;
                case (TrajectoryReflectionType.planeProjecting):
                    ProjectTrajectory(hit.normal, ref _externalForces);
                    break;
            }
            _expiredCollisionTime = 0;
        }
    }

    // все 3 последующих метода сделать позже через делегаты
    private void ReflectTrajectory(Vector3 normal, ref Vector3 currentTrajectory)
    {
        Vector3 reflectedTrajectory = Vector3.Reflect(currentTrajectory, normal);
        float speedLose = Mathf.Pow(Mathf.Sin(Mathf.Deg2Rad * Vector3.Angle(reflectedTrajectory, normal)), _sinWaveSquizing);

        currentTrajectory = reflectedTrajectory * speedLose;
    }

    private void ProjectTrajectory(Vector3 normal, ref Vector3 currentTrajectory)
    {
        Vector3 projectedTragectory = Vector3.ProjectOnPlane(currentTrajectory, normal);

        currentTrajectory = projectedTragectory * _planeProjectingPower;
    }

    private void CalculateCurrentGravityVector(ref Vector3 gravityVector)
    {
        if (groundChecker.getGroundState() == GroundChecker.groundState.onGround)
            gravityVector = Vector3.zero;
        else
        {
            gravityVector = _freeFallAcceleration * Vector3.down;
        }
    }

    private IEnumerator Teleport(Vector3 position, float duration)
    {
        _isMovementAvaliable = false;
        yield return new WaitForSeconds(duration / 2);
        transform.position = position;
        yield return new WaitForSeconds(duration / 2);
        _isMovementAvaliable = true;
    }

    public void EnableControl()
    {
        _isMovementAvaliable = true;
        _camera.SetActive(true);
    }

    private enum TrajectoryReflectionType
    {
        bounce,
        planeProjecting,
        realisticBounce
    }
}

