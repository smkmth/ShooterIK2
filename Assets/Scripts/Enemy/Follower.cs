using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : Character
{
    
    public float tooFarDistance;
    public float tooCloseDistance;
    public float attackTime;
    public float attackRecoverTime;
    public float attackWindupTime;
    public HitDetect leftAttack;
    public HitDetect rightAttack;
    public bool playerIsRight;
    public bool attacking;
    public float rightFudgeDistance;
    public float leftFudgeDistance;
    // Update is called once per frame
    public virtual void Update()
    {
        if (seenPlayer)
        {
            if (canMove)
            {
                float dist = Vector3.Distance(transform.position, player.transform.position);
                playerIsRight = (player.transform.position.x > transform.position.x);
                if (dist > tooFarDistance)
                {
                    if (playerIsRight)
                    {
                        transform.position += Vector3.right * Time.deltaTime;

                    }
                    else
                    {
                        transform.position += -Vector3.right * Time.deltaTime;

                    }


                }
                else if (dist < tooCloseDistance)
                {
                    if (playerIsRight)
                    {
                        transform.position += -Vector3.right * Time.deltaTime;

                    }
                    else
                    {
                        transform.position += Vector3.right * Time.deltaTime;

                    }


                }
                else
                {

                    StartAttack();
                }
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

    public IEnumerator Attack()
    {
        attacking = true;
        canMove = false;
        Debug.Log("Started attack");
        yield return new WaitForSeconds(attackWindupTime);
        Debug.Log("attacking");

        if (playerIsRight)
        {
            rightAttack.isActive = true;
        }
        else
        {
            leftAttack.isActive = true;
        }
        yield return new WaitForSeconds(attackTime);
        Debug.Log("finished attacking");
        leftAttack.isActive = false;
        rightAttack.isActive = false;
        yield return new WaitForSeconds(attackRecoverTime);
        Debug.Log("return to moving");
        canMove = true;
        attacking = false;


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
