using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour

{
   
    public LayerMask hitmask;
    public float ExplosiveForce;
    public float ExplosiveRange;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {

            Collider[] results = Physics.OverlapSphere(collision.GetContact(0).point, ExplosiveRange, hitmask);

            foreach (Collider collider in results)
            {
                if (collider.transform.gameObject.GetComponent<Rigidbody>())
                {
                    if (collider.transform.position.x > transform.position.x)
                    {
                        collider.transform.gameObject.GetComponent<Rigidbody>().AddExplosionForce(ExplosiveForce, transform.position, ExplosiveRange, 100.0f);
                    }
                    else
                    {
                        collider.transform.gameObject.GetComponent<Rigidbody>().AddExplosionForce(ExplosiveForce, transform.position, 300);

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
