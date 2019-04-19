using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int health;
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


    void KillEnemy()
    {
        Destroy(gameObject);
    }

}
