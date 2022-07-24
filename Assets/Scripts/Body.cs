using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    [SerializeField] protected float _health = 0.0f;
    protected float _healthAtStart;



    public virtual void TakeDamage(float damage)
    {
        _health -= damage;
    }
}
