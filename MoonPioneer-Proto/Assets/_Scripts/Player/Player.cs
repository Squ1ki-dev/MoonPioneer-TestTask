using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public partial class Player : MonoBehaviour
{
    [SerializeField] private FloatingJoystick _joystick;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;

    private Rigidbody _rigidbody;
    private Vector3 _moveVector;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }
}
