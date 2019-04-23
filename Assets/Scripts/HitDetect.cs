using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetect : MonoBehaviour
{

    public LayerMask layerToDetect;
   
    

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.layer + "     "+ System.Convert.ToString(other.gameObject.layer, 2));
        //Debug.Log("detect " + layerToDetect.value + "     " + System.Convert.ToString(layerToDetect.value, 2));
       
        if (layerToDetect.value == (layerToDetect.value | (1 << other.gameObject.layer)))
        {
            Debug.Log("hit");
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
            other.gameObject.GetComponent<Enemy>().TakeDamage(10, forcehit);
            Debug.Log(other.gameObject.GetComponent<Enemy>().Health);


        }
    }
}
