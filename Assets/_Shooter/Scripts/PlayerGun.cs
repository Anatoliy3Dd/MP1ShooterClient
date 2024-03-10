using UnityEngine;

public class PlayerGun : Gun
{
    [SerializeField] private Transform _bulletPoint;
    [SerializeField] private float _bulletSpeed = 20f;
    [SerializeField] private float _shootDelay = 0.1f;

    private float _lastShootTime;

    public bool TryShoot(out ShootInfo info) {
        info = new ShootInfo();

        if (Time.time - _lastShootTime < _shootDelay) return false;

        Vector3 position = _bulletPoint.position;
        Vector3 velocity = _bulletPoint.forward * _bulletSpeed;

        Instantiate(_bulletPrefab, _bulletPoint.position, _bulletPoint.rotation).Init(velocity);
        _lastShootTime = Time.time;
        OnShoot?.Invoke();

        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.dX = velocity.x;
        info.dY = velocity.y;
        info.dZ = velocity.z;

        return true;
    }
}
