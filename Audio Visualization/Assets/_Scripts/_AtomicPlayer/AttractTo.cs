using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractTo : MonoBehaviour
{
    Rigidbody _rigidbody;
    public Transform _attractTo;
    public float _strengthOfAttraction, _maxMagnitude;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_attractTo != null)
        {
            Vector3 direction = _attractTo.position - transform.position;
            _rigidbody.AddForce(direction * _strengthOfAttraction);

            if (_rigidbody.velocity.magnitude > _maxMagnitude)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * _maxMagnitude;
            }
        }
    }
}
