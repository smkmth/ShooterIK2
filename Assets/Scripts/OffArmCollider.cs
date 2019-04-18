using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffArmCollider : MonoBehaviour
{

    public Player player;



    private void OnTriggerEnter(Collider other)
    {
        if (!player.Grounded)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Corner"))
            {
                player.OffArmColliderHit(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Corner"))
        {
            player.OffArmColliderExit(other);
        }
    }
}
