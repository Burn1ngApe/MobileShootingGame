using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
    [SerializeField] private GameObject _gun, _bullet, _endOfTheGun;
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private EnemyGeneral _enemyGeneral;
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private float _shootingDistance;

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



    private void Start()
    {
        _attackMode = AttackMode.Idle;
    }



    private void Update()
    {
        Shooting();
    }



    private void Shooting()
    {
        if(_attackMode == AttackMode.Attack && _enemyGeneral._enemies.Count != 0)
        {
            Vector3 enemyPoint = GetClosestEnemyPosition();
            enemyPoint.y = 0;

            if (enemyPoint.magnitude <= _shootingDistance)
            {
                //rotate character towards the enemy
                _characterMovement._rotateToEnemy = true;
                _characterMovement.RotateCharacter(-enemyPoint);

                //shooting projectiles
                if (Time.time > _generalTime && enemyPoint != null)
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



    private Vector3 GetClosestEnemyPosition()
    {
        Vector3 enemyPosition = new Vector3();

        for(int i =0; i < _enemyGeneral._enemies.Count; i++)
        {           
            var distance = transform.position - _enemyGeneral._enemies[i].transform.position;
          
            if(distance.magnitude <= enemyPosition.magnitude || enemyPosition.magnitude == 0)
            {
                enemyPosition = distance;
            }          
        }

        return enemyPosition;
    }



    private void ShootProjectiles()
    {
        //spawn multiple projectiles
        for (int i = 0; i < _projectileAmount; i++)
        {
            //spawn projectile
            var projectile = Instantiate(_bullet);

            //set projectile position to the end of the gun
            projectile.transform.position = _endOfTheGun.transform.position;

            //change rotation of the projectile with little offset
            var newForward = transform.forward;
            newForward.x += Random.Range(-_projectileOffset, _projectileOffset);

            projectile.transform.rotation = Quaternion.LookRotation(newForward);

            //adding force to the projectile
            var rb_projectile = projectile.GetComponent<Rigidbody>();
            rb_projectile.AddForce(projectile.transform.forward * _projectileSpeed, ForceMode.Impulse);

            //destroy projectile after certain time
            Destroy(projectile, _projectileDeathTime);
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
