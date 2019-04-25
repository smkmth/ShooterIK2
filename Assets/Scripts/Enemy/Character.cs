using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    protected Rigidbody rb;
    protected HitDetect thisHitDetect;
    protected Player player;

    private bool takingDamage = false;
    public int MaxHealth;

    protected bool seenPlayer;
    protected bool canMove = true;

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
                KillCharacter();
            }
        }
    }
    public virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("No Player!");
        }
        Health = MaxHealth;
        rb = GetComponent<Rigidbody>();
        thisHitDetect = GetComponent<HitDetect>();
        thisHitDetect.isActive = true;
        Health = MaxHealth;


    }



    public virtual void TakeDamage(int damage)
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



    public virtual void TakeDamage(int damage, Vector3 knockback)
    {

        if (!takingDamage)
        {
            rb.AddForce(knockback, ForceMode.Impulse);
            Health -= damage;
            StopCoroutine(HitFlash());
            StartCoroutine(HitFlash());
        }        
    }


    virtual public void KillCharacter()
    {
        Destroy(gameObject);
    }

    public virtual void Alert()
    {
        Debug.Log(" EnemyAlerted");
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
