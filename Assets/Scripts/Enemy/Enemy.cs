using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemey : Character
{
    
    
    public float attackTime;
    public float attackRecoverTime;
    public float attackWindupTime;
    public HitDetect leftAttack;
    public HitDetect rightAttack;

    protected bool playerIsRight;
    protected bool attacking;
    protected float dist;


    // Update is called once per frame
    public virtual void Update()
    {
        if (seenPlayer)
        {
            if (canMove)
            {
                dist = Vector3.Distance(transform.position, player.transform.position);
                playerIsRight = (player.transform.position.x > transform.position.x);
              
            }
        }


    }

    public virtual void StartAttack()
    {
        Debug.Log("good");
        if (!attacking)
        {
            StopCoroutine(Attack());
            StartCoroutine(Attack());

        }

    }

    public virtual IEnumerator Attack()
    {
        yield return null;

    }
   
    public override void Alert()
    {
        base.Alert();
        seenPlayer = true;



    }
    public override void Forget()
    {
        base.Forget();
        seenPlayer = false;

    }


    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        seenPlayer = true;

    }

    public override void TakeDamage(int damage, Vector3 knockback)
    {
        base.TakeDamage(damage, knockback);
        seenPlayer = true;
    }





}
