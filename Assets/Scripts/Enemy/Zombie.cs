using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    protected Player player;
    protected HitDetect thisHitDetect;
    protected bool seenPlayer;
    protected bool canMove = true;
    


    public virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("No Player!");
        }
        Health = 40;
        rb = GetComponent<Rigidbody>();
        thisHitDetect = GetComponent<HitDetect>();
        thisHitDetect.isActive = true;


    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (seenPlayer)
        {
            if (canMove)
            {
                if (player.transform.position.x > (transform.position.x + .4))
                {
                    transform.position += Vector3.right * Time.deltaTime;

                }
                else if (player.transform.position.x < (transform.position.x - .4))
                {
                    transform.position += -Vector3.right * Time.deltaTime;


                }
                else if (player.transform.position.x > (transform.position.x - .2))
                {
                    Debug.Log("To Close!");
                    transform.position += -Vector3.right * Time.deltaTime;

                }
                else if (player.transform.position.x > (transform.position.x + .2))
                {
                    Debug.Log("To Close!");
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

    

}
