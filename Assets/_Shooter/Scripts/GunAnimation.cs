using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    private const string shoot = "Shoot";

    [SerializeField] private Gun _gun;
    [SerializeField] private Animator _animator;

    private void OnEnable() {
        _gun.OnShoot += Shoot;
    }

    private void OnDisable() {
        _gun.OnShoot -= Shoot;
    }

    private void Shoot() {
        _animator.SetTrigger(shoot);
    }
}
