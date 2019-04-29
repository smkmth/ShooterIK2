using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFollower : Enemey
{

    public float moveSpeed;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        seenPlayer = true;
        canMove = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
    }
}
