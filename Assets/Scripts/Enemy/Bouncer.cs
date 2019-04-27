using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : Enemey
{
    private float timer;
    public float JumpTimer;
    public float jumpDistance;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (timer <= 0)
        {
            rb.AddForce(Vector3.up * jumpDistance);
            timer = JumpTimer;
        }
        else
        {
            timer -= Time.deltaTime;

        }
    }
}
