using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterController : Body
{
    [SerializeField] private CharacterShooting _characterShooting;
    [SerializeField] private CharacterMovement _characterMovement;

    [SerializeField] private GameObject _healthBarCanvas;
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private TMP_Text _allGems;

    [HideInInspector] public int _gemsCollected = 0, _generalAmountOfCollectedGems =0;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    [SerializeField] private UIController _uiController;



    private void Start()
    {
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

        if (_health <= 0)
        {
            Death();
        }
    }



    private void Death()
    {
        _uiController._exitMenuButton.interactable = false;
        _uiController.EnterMenu();
    }



    public void CollectGem()
    {
        _gemsCollected++;
    }



    public void ResetEverything()
    {
        //reset gem amount
        _gemsCollected = 0;
        _generalAmountOfCollectedGems = 0;
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
        _characterShooting._attackMode = CharacterShooting.AttackMode.Idle;
        _characterShooting.ExitAttackMode();

        //let player rotate by joystick input
        _characterMovement._rotateToEnemy = false;

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
        _characterShooting.EnterAttackMode();

        _gemsCollected = 0;
    }



    public void CharacterOnBase()
    {
        _characterShooting.ExitAttackMode();

        _generalAmountOfCollectedGems += _gemsCollected;
        _allGems.text = _generalAmountOfCollectedGems.ToString();
    }
}
