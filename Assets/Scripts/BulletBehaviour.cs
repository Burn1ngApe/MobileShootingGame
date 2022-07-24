using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float _damage;

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject;

        if (enemy.CompareTag("Enemy"))
        {
            enemy.GetComponent<EnemyBehaviour>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
