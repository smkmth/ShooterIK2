using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheSwing : Enemy
{
    public Animator swingAnimator;

    public Player player;
    public bool facingRight;


    private Vector3 flip = new Vector3(-.2f, .2f, .2f);
    private Vector3 notflipped = new Vector3(.2f, .2f, .2f);

    private void Start()
    {
        Health = 100;
        player = GameObject.Find("Player").GetComponent<Player>();

    }

    public void Update()
    {
        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = flip;
            facingRight = false;
        }
        else
        {

            transform.localScale = notflipped;
            facingRight = true;

        }

    }
}
