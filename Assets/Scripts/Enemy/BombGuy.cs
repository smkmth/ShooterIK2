using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGuy : Enemy
{
    float timer;
    public float ThrowTime;
    public GameObject bombPrefab;
    public Vector3 throwArc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
    void ThrowBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, transform);
        bomb.GetComponent<Rigidbody>().AddForce(throwArc, ForceMode.Impulse);


    }
}
