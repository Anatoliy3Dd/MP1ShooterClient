using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFly : MonoBehaviour
{
    public bool IsFly { get; private set; }

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _radius = 0.2f;
    [SerializeField] private float _coyoteTime = 0.15f;

    private float _flyTimer = 0f;

    private void Update() {
        if (Physics.CheckSphere(transform.position, _radius, _layerMask)) {
            IsFly = false;
            _flyTimer = 0f;
        }
        else {
            _flyTimer += Time.deltaTime;
            if (_flyTimer > _coyoteTime) {
                IsFly = true;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
#endif
}
