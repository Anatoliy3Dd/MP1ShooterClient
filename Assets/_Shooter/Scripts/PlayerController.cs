using GameDevWare.Serialization;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShootInfo
{
    public string key;
    public float pX;
    public float pY;
    public float pZ;
    public float dX;
    public float dY;
    public float dZ;
}

[System.Serializable]
public struct CrouchInfo
{
    public string key;
    public string state;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerCharacter;
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private float _mouseSensetivity = 2f;

    private MultiplayerManager _multiplayerManager;

    private void OnEnable() {
        _playerCharacter.OnCrouch += SendCrouch;
        _playerCharacter.OnUncrouch += SendUncrouch;
    }

    private void OnDisable() {
        _playerCharacter.OnCrouch -= SendCrouch;
        _playerCharacter.OnUncrouch -= SendUncrouch;
    }

    private void Start() {
        _multiplayerManager = MultiplayerManager.Instance;
    }

    private void Update() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        bool isShoot = Input.GetMouseButton(0);
        bool space = Input.GetKeyDown(KeyCode.Space);

        _playerCharacter.SetInput(h, v, mouseX * _mouseSensetivity);
        _playerCharacter.RotateX(-mouseY * _mouseSensetivity);

        if (isShoot && _gun.TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);

        if (space) _playerCharacter.Jump();

        if (Input.GetKeyDown(KeyCode.LeftControl)) _playerCharacter.Crouch();
        if (Input.GetKeyUp(KeyCode.LeftControl)) _playerCharacter.Uncrouch();

        SendMove();
    }

    private void SendShoot(ref ShootInfo shootInfo) {
        shootInfo.key = _multiplayerManager.GetSessionId();
        string json = JsonUtility.ToJson(shootInfo);
        _multiplayerManager.SendMessage("shoot", json);
    }

    private void SendCrouch() {
        CrouchInfo crouchInfo = new() {
            key = _multiplayerManager.GetSessionId(),
            state = "crouch"
        };
        string json = JsonUtility.ToJson(crouchInfo);
        _multiplayerManager.SendMessage("crouch", json);
    }

    private void SendUncrouch() {
        CrouchInfo crouchInfo = new() {
            key = _multiplayerManager.GetSessionId(),
            state = "uncrouch"
        };
        string json = JsonUtility.ToJson(crouchInfo);
        _multiplayerManager.SendMessage("crouch", json);
    }

    private void SendMove() {
        _playerCharacter.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);
        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"pX", position.x },
            {"pY", position.y },
            {"pZ", position.z },
            {"vX", velocity.x },
            {"vY", velocity.y },
            {"vZ", velocity.z },
            {"rX", rotateX},
            {"rY", rotateY}
        };
        _multiplayerManager.SendMessage("move", data);
    }
}
