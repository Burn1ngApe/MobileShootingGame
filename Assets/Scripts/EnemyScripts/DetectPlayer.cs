using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DetectPlayer : MonoBehaviour
{
    [Inject]
    private CharacterController _characterController;

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
