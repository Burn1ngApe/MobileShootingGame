using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Body
{
    [HideInInspector] public EnemyGeneral _enemyGeneral;
    [SerializeField] private int _gemDropChance;
    [SerializeField] private GameObject _gemInstance;

    [SerializeField] private float _sightRange, _attackRange, _moveSpeed;

    [SerializeField] private LayerMask _playerLM;
    private bool _playerSpotted = false;
    [HideInInspector] public GameObject _player;
    private CharacterController _characterController;

    private Rigidbody _rb;

    [SerializeField] private Animator _anim;
    private float _generalTime = 0.0f;
    [SerializeField] private float _timeBetweenAttacks, _minDamage, _maxDamage;
    



    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _characterController = _player.GetComponent<CharacterController>();
    }



    private void Update()
    {
        var toPlayerDir = _player.transform.position - transform.position;

        Ray ray = new Ray(transform.position, toPlayerDir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, _playerLM))
        {
            //if character is between sightrange and attackrange than run to the player
            if (hit.distance < _sightRange && hit.distance > 1f) RunForPlayer(toPlayerDir);

            //if player is in attackrrange than attack
            if (hit.distance < _attackRange) AttackPlayer();
        }
    }



    private void RunForPlayer(Vector3 toPlayerDir)
    {
        _anim.SetTrigger("Running");

        //rotate enemy towards player
        var rot = Quaternion.LookRotation(_player.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 250 * Time.deltaTime);

        //move enemy towards player
        _rb.velocity = toPlayerDir.normalized * _moveSpeed;
    }



    private void AttackPlayer()
    {
        if (Time.time > _generalTime)
        {
            _generalTime = Time.time + _timeBetweenAttacks;
            _anim.SetTrigger("Throw");

            float randomDamage = Random.Range(_minDamage, _maxDamage);

            _characterController.TakeDamage(randomDamage);
        }
    }


    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (_health <= 0)
        {
            Death();
        }
    }



    private void DropGem()
    {
        var randomInt = Random.Range(0, 100);

        if (randomInt <= _gemDropChance)
        {
            var gem = Instantiate(_gemInstance);

            var newPos = transform.position;
            newPos.y = 1.5f;
            gem.transform.position = newPos;
        }
    }



    private void Death()
    {
        _enemyGeneral._enemies.Remove(gameObject);
        DropGem();
        Destroy(gameObject);
    }
}
