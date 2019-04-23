using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody rb;

    protected int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (health <= 0)
            {
                Debug.Log("hit enemy health = " + health);
                KillEnemy();
            }
        }
    }

    public void Start()
    {
        Health = 100;
        GetComponent<Rigidbody>();
        
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

    }

    public void TakeDamage(int damage, Vector3 knockback)
    {
        Health -= damage;
        rb.AddForce(knockback, ForceMode.Impulse);
    }


    void KillEnemy()
    {
        Destroy(gameObject);
    }

}
