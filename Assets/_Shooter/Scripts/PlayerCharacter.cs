using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private float _jumpForce = 50f;
    [SerializeField] private float _minHeadAngle = -8f;
    [SerializeField] private float _maxHeadAngle = 70f;
    [SerializeField] private float _jumpDelay = 0.2f;

    private float _inputH;
    private float _inputV;
    private float _rotateY;
    private float _currentRotateX;
    private float _jumpTime;
    private bool _inCrouch;

    private void Start() {
        Transform camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;
    }

    public void SetInput(float h, float v, float rotateY) {
        _inputH = h;
        _inputV = v;
        _rotateY += rotateY;
    }

    private void FixedUpdate() {
        Move();
        RotateY();
    }

    private void Move() {
        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * Speed;
        velocity.y = _rigidbody.velocity.y;
        Velocity = velocity;
        _rigidbody.velocity = Velocity;
    }

    private void RotateY() {
        _rigidbody.angularVelocity = new Vector3(0f, _rotateY, 0f);
        _rotateY = 0f; 
    }

    public void RotateX(float value) {
        _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_currentRotateX, 0f, 0f);
    }

    public void Jump() {
        if (_checkFly.IsFly) return;
        if (Time.time - _jumpTime < _jumpDelay) return;

        _rigidbody.AddForce(0f, _jumpForce, 0f, ForceMode.VelocityChange);
        _jumpTime = Time.time;
    }

    public void Crouch() {
        if (_checkFly.IsFly) return;

        OnCrouch?.Invoke();
        _inCrouch = true;
    }

    public void Uncrouch() {
        if (!_inCrouch) return;

        OnUncrouch?.Invoke();
        _inCrouch = false;
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY) {
        position = transform.position;
        velocity = _rigidbody.velocity;

        rotateX = _head.localEulerAngles.x;
        rotateY = transform.eulerAngles.y;
    }
}
