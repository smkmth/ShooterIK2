using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : Zombie
{
    public override void Start()
    {
        base.Start();
        thisHitDetect.isActive = false;
        Health = 100;

    }

    public override void Alert()
    {
        base.Alert();
        seenPlayer = true;
        thisHitDetect.isActive = true;



    }
    public override void Forget()
    {
        base.Alert();
        thisHitDetect.isActive = false;



    }
}
