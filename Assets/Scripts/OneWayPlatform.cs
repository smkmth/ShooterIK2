using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{

    public BoxCollider platform;
    public Collider player;
    public LayerMask layerToEffect;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Collider>();
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (layerToEffect.value == (layerToEffect.value | (1 << other.gameObject.layer)))
        {
            Physics.IgnoreCollision(platform, player, true);
            Debug.Log("this");
            // platform.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (layerToEffect.value == (layerToEffect.value | (1 << other.gameObject.layer)))
        {
            Physics.IgnoreCollision(platform, player, false);
            Debug.Log("exit");
            // platform.enabled = false;
        }
    }
}
