using System.Collections;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private const string Grounded = "Grounded";
    private const string Speed = "Speed";

    [SerializeField] private Animator _animator;
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private Character _character;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _body;
    [SerializeField] private CapsuleCollider _capsule;

    private Vector3 _bodyUncrouchPosition;
    private Vector3 _bodyUncrouchScale = Vector3.one;
    private Vector3 _headUncrouchPosition;
    private Vector3 _bodyCrouchPosition;
    private Vector3 _bodyCrouchScale = new(1f, 1f, 0.5f);
    private Vector3 _headCrouchPosition;
    private Vector3 _capsuleUncrouchCenter = new(0f, 1f, 0f);
    private Vector3 _capsuleCrouchCenter = new(0f, 0.7f, 0f);
    private float _capsuleUncrouchHeight = 2f;
    private float _capsuleCrouchHeight = 1.4f;
    private float _crouchAnimTime = 0.15f;

    private void OnEnable() {
        _character.OnCrouch += PlayCrouchAnimation;
        _character.OnUncrouch += PlayUncrouchAnimation;
    }

    private void OnDisable() {
        _character.OnCrouch -= PlayCrouchAnimation;
        _character.OnUncrouch -= PlayUncrouchAnimation;
    }

    private void Start() {
        _bodyUncrouchPosition = _body.localPosition;
        _headUncrouchPosition = _head.localPosition;

        _bodyCrouchPosition = _bodyUncrouchPosition + new Vector3(0f, 0f, -0.15f);
        _headCrouchPosition = _headUncrouchPosition + new Vector3(0f, 0f, -0.65f);
    }

    private void Update() {
        Vector3 localVelocity = _character.transform.InverseTransformVector(_character.Velocity);
        float speed = localVelocity.magnitude / _character.Speed;
        float sign = Mathf.Sign(localVelocity.z);
        _animator.SetFloat(Speed, speed * sign);
        _animator.SetBool(Grounded, !_checkFly.IsFly);
    }

    private void PlayCrouchAnimation() {
        StopAllCoroutines();
        StartCoroutine(CrouchAnimation());
    }

    private void PlayUncrouchAnimation() {
        StopAllCoroutines();
        StartCoroutine(UncrouchAnimation());
    }

    private IEnumerator CrouchAnimation() {
        Vector3 bodyCurrentPosition = _body.localPosition;
        Vector3 bodyCurrentScale = _body.localScale;
        Vector3 headCurrentPosition = _head.localPosition;
        Vector3 capsuleCurrentCenter = _capsule.center;
        float capsuleCurrentHeight = _capsule.height;

        for (float t = 0; t < 1f; t += Time.deltaTime / _crouchAnimTime) {
            _body.localPosition = Vector3.Lerp(bodyCurrentPosition, _bodyCrouchPosition, t);
            _body.localScale = Vector3.Lerp(bodyCurrentScale, _bodyCrouchScale, t);
            _head.localPosition = Vector3.Lerp(headCurrentPosition, _headCrouchPosition, t);
            _capsule.center = Vector3.Lerp(capsuleCurrentCenter, _capsuleCrouchCenter, t);
            _capsule.height = Mathf.Lerp(capsuleCurrentHeight, _capsuleCrouchHeight, t);
            yield return null;
        }
        _body.localPosition = _bodyCrouchPosition;
        _body.localScale = _bodyCrouchScale;
        _head.localPosition = _headCrouchPosition;
        _capsule.center = _capsuleCrouchCenter;
        _capsule.height = _capsuleCrouchHeight;
    }

    private IEnumerator UncrouchAnimation() {
        Vector3 bodyCurrentPosition = _body.localPosition;
        Vector3 bodyCurrentScale = _body.localScale;
        Vector3 headCurrentPosition = _head.localPosition;
        Vector3 capsuleCurrentCenter = _capsule.center;
        float capsuleCurrentHeight = _capsule.height;

        for (float t = 0; t < 1f; t += Time.deltaTime / _crouchAnimTime) {
            _body.localPosition = Vector3.Lerp(bodyCurrentPosition, _bodyUncrouchPosition, t);
            _body.localScale = Vector3.Lerp(bodyCurrentScale, _bodyUncrouchScale, t);
            _head.localPosition = Vector3.Lerp(headCurrentPosition, _headUncrouchPosition, t);
            _capsule.center = Vector3.Lerp(capsuleCurrentCenter, _capsuleUncrouchCenter, t);
            _capsule.height = Mathf.Lerp(capsuleCurrentHeight, _capsuleUncrouchHeight, t);
            yield return null;
        }
        _body.localPosition = _bodyUncrouchPosition;
        _body.localScale = _bodyUncrouchScale;
        _head.localPosition = _headUncrouchPosition;
        _capsule.center = _capsuleUncrouchCenter;
        _capsule.height = _capsuleUncrouchHeight;
    }
}
