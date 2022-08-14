using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected float _health = 0.0f;
    protected float _healthAtStart;



    public virtual void TakeDamage(float damage)
    {
        _health -= damage;
        if(_health < 0) { Dead(); }
    }



    public abstract void Dead();

}
