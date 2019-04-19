using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetect : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.transform.gameObject.GetComponent<Player>().Health -= 10;
            Debug.Log(collision.transform.gameObject.GetComponent<Player>().Health);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Vector3 forcehit = new Vector3();
            if (other.transform.position.x > transform.position.x)
            {
                forcehit.x = 5;
                forcehit.y = 3;
                forcehit.z = 0;
               
            } else
            {

                forcehit.x = -5;
                forcehit.y = 3;
                forcehit.z = 0;
            }
            other.gameObject.GetComponent<Player>().TakeDamage(10, forcehit);
            Debug.Log(other.gameObject.GetComponent<Player>().Health);


        }
    }
}
