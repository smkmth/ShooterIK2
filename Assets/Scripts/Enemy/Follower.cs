using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : Enemey
{
    
    public float tooFarDistance;
    public float tooCloseDistance;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

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

    public override IEnumerator Attack()
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
}
