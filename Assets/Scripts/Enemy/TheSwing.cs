using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheSwing : Character
{
    public Animator swingAnimator;

    public Player player;
    public bool facingRight;


    public HitDetect mainHand;
    public int MainHandDamage;
    public HitDetect offHand;
    public int OffHandDamage;
    public HitDetect mainFoot;
    public int MainFootDamage;
    public HitDetect offFoot;
    public int OffFootDamage;
    public List<HitDetect> hitDetects;

    private Vector3 flip = new Vector3(-.2f, .2f, .2f);
    private Vector3 notflipped = new Vector3(.2f, .2f, .2f);

    private void Start()
    {
        hitDetects.Add(mainFoot);
        hitDetects.Add(offFoot);
        hitDetects.Add(mainHand);
        hitDetects.Add(offHand);
        foreach(HitDetect detect in hitDetects)
        {
            detect.isActive = true;
        }
        mainFoot.damage = MainFootDamage;
        mainHand.damage = MainHandDamage;
        offFoot.damage = OffFootDamage;
        offHand.damage = OffHandDamage;
        Health = 100;
        player = GameObject.Find("Player").GetComponent<Player>(); 
        rb = GetComponent<Rigidbody>();


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
