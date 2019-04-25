using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody rb;
    private bool takingDamage = false;
    public int MaxHealth;
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
            Debug.Log("hit enemy health = " + health);
            if (health <= 0)
            {
                KillEnemy();
            }
        }
    }




    public void TakeDamage(int damage)
    {

        Health -= damage;
        StopCoroutine(HitFlash());
        StartCoroutine(HitFlash());

    }

    IEnumerator HitFlash()
    {
        takingDamage = true;
    
        yield return new WaitForSeconds(.3f);
        takingDamage = false;


    }



    public void TakeDamage(int damage, Vector3 knockback)
    {

        if (!takingDamage)
        {
            rb.AddForce(knockback, ForceMode.Impulse);
            Health -= damage;
            StopCoroutine(HitFlash());
            StartCoroutine(HitFlash());
        }        
    }


    virtual public void KillEnemy()
    {
        Destroy(gameObject);
    }

    public virtual void Alert()
    {
        Debug.Log("Enemy Alerted");
    }

    public virtual void StopMoving()
    {
        Debug.Log("Enemy stopped");
    }

    public virtual void Forget()
    {
        Debug.Log("Enemy Forgotten");
    }
   
}
