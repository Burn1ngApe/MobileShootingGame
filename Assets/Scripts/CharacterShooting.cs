using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterShooting : MonoBehaviour
{
    [SerializeField] private GameObject _gun, _bullet, _endOfTheGun;
    [SerializeField] private Animator _characterAnimator;

    [SerializeField] private CharacterMovement _characterMovement;
    public float _shootingDistance;

    private ObjectPool _objectPool;

    public enum AttackMode
    {
        Idle,
        Attack
    }

    [HideInInspector]
    public AttackMode _attackMode;

    [SerializeField] private float _timeBetweenAttacks;
    private float _generalTime;

    [SerializeField] private float _projectileAmount, _projectileOffset, _projectileSpeed, _projectileDeathTime;


    private Vector3 _closestEnemyPosition = Vector3.zero;
    private Vector3 _previousEnemyPosition = Vector3.zero;


   [HideInInspector] public bool _enemyInSight = false;

    private void Start()
    {
        _objectPool = GetComponent<ObjectPool>();

        _attackMode = AttackMode.Idle;
    }



    private void Update()
    {
        Shooting();
    }



    private void Shooting()
    {
        if(_attackMode == AttackMode.Attack)
        {
            if (_enemyInSight)
            {
                //rotate character towards the enemy
                _characterMovement._rotateToEnemy = true;
                _characterMovement.RotateCharacter(-_closestEnemyPosition);

                //shooting projectiles
                if (Time.time > _generalTime && _closestEnemyPosition != null)
                {
                    _generalTime = Time.time + _timeBetweenAttacks;
                    ShootProjectiles();
                }
            }
            else
            {
                _characterMovement._rotateToEnemy = false;
            }
        }
        else
        {
            _characterMovement._rotateToEnemy = false;
        }
    }



    public void AngerPlayer(Vector3 dist, Vector3 enemyPosition)
    {
        var d = transform.position - _previousEnemyPosition;

        if (_closestEnemyPosition.magnitude == 0 || dist.magnitude < d.magnitude)
        {
            _enemyInSight = true;

            _closestEnemyPosition = dist;
            _previousEnemyPosition = enemyPosition;
        }
    }



    private void ShootProjectiles()
    {
        _objectPool.CleanPool();



        //spawn multiple projectiles
        for (int i = 0; i < _projectileAmount; i++)
        {
            //get rotation
            var newForward = transform.forward;
            newForward.x += Random.Range(-_projectileOffset, _projectileOffset);

            var projPos = _endOfTheGun.transform.position;
            var projRot = Quaternion.LookRotation(newForward);

            _objectPool.GetObjectFromPool(projPos, projRot, _projectileSpeed);
        }
    }



    public void EnterAttackMode()
    {
        _attackMode = AttackMode.Attack;

        _gun.SetActive(true);
        _characterAnimator.SetLayerWeight(1, 1);
    }



    public void ExitAttackMode()
    {       
        _attackMode = AttackMode.Idle;

        _gun.SetActive(false);
        _characterAnimator.SetLayerWeight(1, 0);
    }
}
