using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour

{
   
    public LayerMask layerToDetect;
    public LayerMask layersToDamage;
    public float ExplosiveForce;
    public float ExplosiveRange;
    public int ExplosiveDamage;


    public void Init(float explosiveForce, float explosiveRange, int explosiveDamage)
    {
        ExplosiveDamage = explosiveDamage;
        ExplosiveForce = explosiveForce;
        ExplosiveRange = explosiveRange;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (layerToDetect.value == (layerToDetect.value | (1 << collision.gameObject.layer)))
        {



            Collider[] results = Physics.OverlapSphere(collision.GetContact(0).point, ExplosiveRange, layersToDamage);

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
                        if (collider.gameObject.GetComponent<HitDetect>())
                        {
                            collider.gameObject.GetComponent<HitDetect>().DoDamageToParent(ExplosiveDamage);
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
