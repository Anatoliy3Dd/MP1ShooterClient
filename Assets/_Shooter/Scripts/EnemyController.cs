using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _character;
    [SerializeField] private EnemyGun _gun;

    private List<float> _receiveTimeInterval = new List<float> {0f, 0f, 0f, 0f, 0f };
    private float _lastReceiveTime = 0f;
    private Player _player;
    private float AverageInterval {
        get {
            int receiveTimeIntervalCount = _receiveTimeInterval.Count;
            float summ = 0f;
            for (int i = 0; i < receiveTimeIntervalCount; i++) {
                summ += _receiveTimeInterval[i];
            }
            return summ / receiveTimeIntervalCount;
        }
    }

    public void Init(Player player) {
        _player = player;
        _character.SetSpeed(_player.speed);
        _player.OnChange += OnChange;
    }

    public void Shoot(in ShootInfo info) {
        Vector3 position = new(info.pX, info.pY, info.pZ);
        Vector3 velocity = new(info.dX, info.dY, info.dZ);
        _gun.Shoot(position, velocity);
    }

    public void CrouchUncrouch(in CrouchInfo info) {
        switch (info.state) {
            case "crouch":
                _character.Crouch();
                break;
            case "uncrouch":
                _character.Uncrouch();
                break;
            default:
                Debug.Log("Ошибка данных приседания");
                break;
        }
    }

    public void Destroy() {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }

    private void SaveReceiveTime() {
        float interval = Time.time - _lastReceiveTime;
        _lastReceiveTime = Time.time;

        _receiveTimeInterval.Add(interval);
        _receiveTimeInterval.Remove(0);
    }

    internal void OnChange(List<DataChange> changes) {
        Vector3 position = _character.TargetPosition;
        Vector3 velocity = _character.Velocity;

        SaveReceiveTime();

        foreach (DataChange dataChange in changes) {
            switch (dataChange.Field) {
                case "pX":
                    position.x = (float)dataChange.Value;
                    break;
                case "pY":
                    position.y = (float)dataChange.Value;
                    break;
                case "pZ":
                    position.z = (float)dataChange.Value;
                    break;
                case "vX":
                    velocity.x = (float)dataChange.Value;
                    break;
                case "vY":
                    velocity.y = (float)dataChange.Value;
                    break;
                case "vZ":
                    velocity.z = (float)dataChange.Value;
                    break;
                case "rX":
                    _character.SetRotateX((float)dataChange.Value);
                    break;
                case "rY":
                    _character.SetRotateY((float)dataChange.Value);
                    break;
                default:
                    Debug.Log("Не обрабатывается изменение поля " + dataChange.Field);
                    break;
            }
        }

        _character.SetMovemnt(position, velocity, AverageInterval);
    }
}
