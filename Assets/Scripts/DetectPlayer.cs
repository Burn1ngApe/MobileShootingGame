using UnityEngine;
using Zenject;

public class DetectPlayer : MonoBehaviour
{
    [Inject]
    private PlayerNestedClass _playerNested;
    private CharacterController _characterController;



    private void Start()
    {
        _characterController = _playerNested.CharacterController;
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _characterController.CharacterOnEnemyField();
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _characterController.CharacterOnBase();
        }
    }

}
