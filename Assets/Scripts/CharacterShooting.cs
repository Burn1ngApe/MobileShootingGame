using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
    [SerializeField] private GameObject _gun, _bullet, _endOfTheGun;
    [SerializeField] private Animator _characterAnimator;

    [SerializeField] private CharacterMovement _characterMovement;
    public float ShootingDistance;

    private ObjectPool _objectPool;

    public enum AttackMode
    {
        Idle,
        Attack
    }

    [HideInInspector]
    public AttackMode attackMode;

    [SerializeField] private float _timeBetweenAttacks;
    private float _generalTime;

    [SerializeField] private float  _projectileOffset, _projectileSpeed, _projectileDeathTime;

    [SerializeField] private int _projectileAmount;


    private Vector3 _closestEnemyPosition = Vector3.zero;
    private Vector3 _previousEnemyPosition = Vector3.zero;


    [HideInInspector] public bool EnemyInSight = false;



    private void Start()
    {
        _objectPool = GetComponent<ObjectPool>();
        _objectPool.CreatePool(_projectileAmount);

        attackMode = AttackMode.Idle;
    }



    private void Update()
    {
        Shooting();
    }



    private void Shooting()
    {
        if (attackMode == AttackMode.Attack)
        {
            if (EnemyInSight)
            {
                //rotate character towards the enemy
                _characterMovement.RotateToEnemy = true;
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
                _characterMovement.RotateToEnemy = false;
            }
        }
        else
        {
            _characterMovement.RotateToEnemy = false;
        }
    }



    public void AngerPlayer(Vector3 dist, Vector3 enemyPosition)
    {
        var d = transform.position - _previousEnemyPosition;

        if (_closestEnemyPosition.magnitude == 0 || dist.magnitude < d.magnitude)
        {
            EnemyInSight = true;

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
        attackMode = AttackMode.Attack;

        _gun.SetActive(true);
        _characterAnimator.SetLayerWeight(1, 1);
    }



    public void ExitAttackMode()
    {
        attackMode = AttackMode.Idle;

        _gun.SetActive(false);
        _characterAnimator.SetLayerWeight(1, 0);
    }
}
