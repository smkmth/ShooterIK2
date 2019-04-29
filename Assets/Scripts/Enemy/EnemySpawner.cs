using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Enemey
{

    public GameObject enemyToSpawn;
    public float spawnTimer;
    public float spawnRate;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        canMove = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (seenPlayer)
        {
            if (spawnTimer >= 0)
            {
                spawnTimer = spawnRate;
                Instantiate(enemyToSpawn, transform);
            }
            else
            {
                spawnTimer -= Time.deltaTime;
            }
        }
        
    }


}
