using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour, ISpawnable
{
    public float _damage;
    [SerializeField] private Rigidbody _rb;

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<IEnemy>() != null)
        {
           DestroyObject(other.gameObject);
        }
    }



    public void SpawnObject(Vector3 pos, Quaternion rot, float force)
    {
        _rb.velocity = Vector3.zero;

        transform.position = pos;
        transform.rotation = rot;

        var newDir = transform.forward * force;
        newDir.y = 0;

        _rb.AddForce(newDir, ForceMode.Impulse);
    }



    public void DestroyObject(GameObject enemy)
    {
        enemy.GetComponent<IEnemy>().SendDamage(_damage);

        gameObject.SetActive(false);
    }
}
