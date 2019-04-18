using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{

    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;

    // Start is called before the first frame update
    void Awake()
    {
        //makes sure that this gameobject and children always exist
        DontDestroyOnLoad(this.gameObject);

        RunTests();

      

    }

    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }


    public void RunTests()
    {
        if (LayerMask.NameToLayer("Ground") == (LayerMask.NameToLayer("Node") | (1 << 11)))
        {
            Debug.LogError("No Ground Layer, we need a 'Ground' layer, and the ground to be tagged as ground!");
        }
        if (LayerMask.NameToLayer("Enemy") == (LayerMask.NameToLayer("Object") | (1 << 12)))
        {
            Debug.LogError("No Layer Enemy, we need an Enemy layer, and every enemy  to be tagged 'Enemy' ");
        }

        Player playerchar = GetComponentInChildren<Player>();
        
        if(playerchar == null)
        {
            Debug.LogError("No player!, we need a player attached to this object ");

        }




    }
}
