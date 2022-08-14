using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterController : Health, IObserver
{
    public CharacterShooting characterShooting;
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private GameObject _healthBarCanvas;
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private TMP_Text _allGems;

    [HideInInspector] public int GemsCollected = 0, GeneralAmountOfCollectedGems =0;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    public UIController uiController;



    private void Start()
    {
        uiController.AddObserver(this);

        _healthAtStart = _health;

        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }



    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        //show health in UI
        var healthForBar = 1f / _healthAtStart * damage;
        _healthBarFill.fillAmount -= healthForBar;

    }



    public override void Dead()
    {
        uiController.ExitMenuButton.interactable = false;
        uiController.EnterMenu();
    }



    public void CollectGem()
    {
        GemsCollected++;
    }



    public void ResetEverything()
    {
        //reset gem amount
        GemsCollected = 0;
        GeneralAmountOfCollectedGems = 0;
        _allGems.text = 0.ToString();

        //destroy spawned gems
        var gems = FindObjectsOfType<GemBehaviour>();

        foreach (var gem in gems)
        {
            Destroy(gem.gameObject);
        }

        //restore characters health
        _health = _healthAtStart;
        _healthBarFill.fillAmount = 1;

        //exit attack mode
        characterShooting.attackMode = CharacterShooting.AttackMode.Idle;
        characterShooting.ExitAttackMode();

        //let player rotate by joystick input
        _characterMovement.RotateToEnemy = false;

        //reset player position
        gameObject.transform.position = _startPosition;
        gameObject.transform.rotation = _startRotation;
    }



    private void LateUpdate()
    {
        ChangeHealthBarCanvasTransform();
    }



    private void ChangeHealthBarCanvasTransform()
    {
        var newPos = transform.position;
        newPos.y = 3f;

        _healthBarCanvas.transform.position = newPos;
    }



    public void CharacterOnEnemyField()
    {
        characterShooting.EnterAttackMode();

        GemsCollected = 0;
    }



    public void CharacterOnBase()
    {
        characterShooting.ExitAttackMode();

        GeneralAmountOfCollectedGems += GemsCollected;
        _allGems.text = GeneralAmountOfCollectedGems.ToString();
    }



    public void UpdateData()
    {
        ResetEverything();
    }
}
