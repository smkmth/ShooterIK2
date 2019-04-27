using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGuy : Enemey
{
    public float ThrowTime;
    float timer;
    public GameObject bombPrefab;
    public Vector3 throwArc;
    public float throwArcMod;
    public float bombExplosiveForce;
    public float bombExplosiveRange;
    public int   bombExplosiveDamage;


    // Start is called before the first frame update
    public override void Start()
    {

        base.Start();
        timer = ThrowTime;

    }

    // Update is called once per frame
    public override void Update()
    {


        if (seenPlayer)
        {
            if (timer <= 0)
            {
                timer = ThrowTime;
                ThrowBomb();
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        
    }
    void ThrowBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, transform);
        bomb.GetComponent<Bomb>().Init(bombExplosiveForce, bombExplosiveRange, bombExplosiveDamage);
        throwArc.x = (player.transform.position.x -transform.position.x);
        if (throwArc.x > 0)
        {
            throwArc.x += throwArcMod;
        }
        else
        {
            throwArc.x -= throwArcMod;

        }
      
        bomb.GetComponent<Rigidbody>().AddForce(throwArc, ForceMode.Impulse);


    }
}
