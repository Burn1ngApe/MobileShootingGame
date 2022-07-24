using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private FixedJoystick _joystick;

    [SerializeField] private float _moveSpeed, _rotationSpeed;
    [SerializeField] private Animator _characterAnimator;

    [HideInInspector] public bool _rotateToEnemy = false;



    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }



    private void FixedUpdate()
    {
        var movement = new Vector3(_joystick.Horizontal * _moveSpeed, _rb.velocity.y, _joystick.Vertical * _moveSpeed);

        _rb.velocity = movement;

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            if (!_rotateToEnemy) RotateCharacter(movement);
            _characterAnimator.SetTrigger("Running");
        }
        else
        {
            _characterAnimator.SetTrigger("DynIdle");
        }
    }



    public void RotateCharacter(Vector3 rotation)
    {
        var rot = Quaternion.LookRotation(rotation);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _rotationSpeed * Time.deltaTime);
    }
}
