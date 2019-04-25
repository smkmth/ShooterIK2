using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : Character
{
    
    public float stopDistance;
    public float tooCloseDistance;
        
    // Update is called once per frame
    public virtual void Update()
    {
        if (seenPlayer)
        {
            if (canMove)
            {
                if (player.transform.position.x > (transform.position.x + stopDistance))
                {
                    transform.position += Vector3.right * Time.deltaTime;

                }
                else if (player.transform.position.x < (transform.position.x - stopDistance))
                {
                    transform.position += -Vector3.right * Time.deltaTime;


                }
                else if (player.transform.position.x > (transform.position.x + tooCloseDistance))
                {
                    Debug.Log("To Close! LEFT");
                    transform.position += -Vector3.right * Time.deltaTime;

                }
                else if (player.transform.position.x < (transform.position.x - tooCloseDistance))
                {
                    Debug.Log("To Close! RIGHT");
                    transform.position += Vector3.right * Time.deltaTime;

                }
            }
        }


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
