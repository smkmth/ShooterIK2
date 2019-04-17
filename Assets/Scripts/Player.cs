using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject MainArmIK;
    Vector3 flip = new Vector3(-1, 1, 1);
    public bool facingRight = true;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x < (Screen.width / 2))
        {
            transform.localScale = flip;
            facingRight = false;

        }else
        {
            transform.localScale = Vector3.one;
            facingRight = true;
        }
        Vector3 pos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        MainArmIK.transform.position = pos;
        float inputLR = Input.GetAxis("Horizontal");
        if (inputLR != 0)
        {
            transform.position += Vector3.right * inputLR * Time.deltaTime ;
        }

    }
}
