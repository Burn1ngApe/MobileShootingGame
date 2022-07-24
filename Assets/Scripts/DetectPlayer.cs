using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private CharacterController _characterController;



    private void Start()
    {
        _characterController = _player.GetComponent<CharacterController>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == _player)
        {
            _characterController.CharacterOnEnemyField();
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player)
        {
            _characterController.CharacterOnBase();
        }
    }

}
