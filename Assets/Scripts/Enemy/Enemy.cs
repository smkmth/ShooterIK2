using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (health <= 0)
            {
                Debug.Log("hit enemy health = " + health);
                KillEnemy();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = 10;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void KillEnemy()
    {
        Destroy(gameObject);
    }

}
