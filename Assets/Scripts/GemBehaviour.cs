using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject;

        if (player.CompareTag("Player"))
        {
            player.GetComponent<CharacterController>().CollectGem();
            Destroy(gameObject);
        }
    }
}
