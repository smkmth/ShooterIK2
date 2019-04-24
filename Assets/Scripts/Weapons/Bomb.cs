using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour

{
   
    public LayerMask hitmask;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {

            Collider[] results = Physics.OverlapSphere(collision.GetContact(0).point, 3.0f, hitmask);

            foreach (Collider collider in results)
            {
                Debug.Log("htaTGASD");
                if (collider.transform.gameObject.GetComponent<Rigidbody>())
                {
                    if (collider.transform.position.x > transform.position.x)
                    {
                        collider.transform.gameObject.GetComponent<Rigidbody>().AddExplosionForce(300.0f, transform.position, 300);
                    }
                    else
                    {
                        collider.transform.gameObject.GetComponent<Rigidbody>().AddExplosionForce(-300.0f, transform.position, 300);

                    }
                    if (collider.gameObject.GetComponent<Player>())
                    {
                        collider.gameObject.GetComponent<Player>().TakeDamage(10);
                    }
                }
            }
            Destroy(gameObject);
        }
        

    }
    void BlowUp()
    {

    }
}
