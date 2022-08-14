using UnityEngine;

public class EnemyBehaviour : Enemy
{
    [SerializeField] private int _gemDropChance;
    [SerializeField] private GameObject _gemInstance;

    [SerializeField] private float _sightRange, _attackRange, _moveSpeed;

    [SerializeField] private LayerMask _playerLM;

    [HideInInspector] public GameObject Player;

    [HideInInspector] public CharacterController CharacterController;
    [HideInInspector] public CharacterShooting CharacterShooting;

    private Rigidbody _rb;

    [SerializeField] private Animator _anim;
    private float _generalTime = 0.0f;
    [SerializeField] private float _timeBetweenAttacks, _minDamage, _maxDamage;



    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }



    private void Update()
    {
        var toPlayerDir = Player.transform.position - transform.position;

        AngerPlayer(toPlayerDir);

        InteractWithPlayer(toPlayerDir);
    }



    private void AngerPlayer(Vector3 toPlayerDir)
    {
        if (Physics.CheckSphere(transform.position, CharacterShooting.ShootingDistance, _playerLM))
        {
            CharacterShooting.AngerPlayer(toPlayerDir, transform.position);
        }
    }



    private void InteractWithPlayer(Vector3 toPlayerDir)
    {
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
        var rot = Quaternion.LookRotation(Player.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 250 * Time.deltaTime);

        //move enemy towards player
        _rb.velocity = toPlayerDir.normalized * _moveSpeed;
    }



    public void AttackPlayer()
    {
        _anim.SetTrigger("Throw");
    }



    public void DeliverDamage()
    {
        float randomDamage = Random.Range(_minDamage, _maxDamage);

        CharacterController.TakeDamage(randomDamage);
    }



    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
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



    public override void Dead()
    {
        CharacterShooting.EnemyInSight = false;
        DropGem();
        Destroy(gameObject);
    }
}
