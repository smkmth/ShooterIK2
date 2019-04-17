using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject MainArmIK;
    public Animator PlayerAnim;

    Vector3 flip = new Vector3(-1, 1, 1);
    public bool facingRight = true;
    public bool running = false;

    public float walkSpeed;
    public float runSpeed;
    public float jumpHeight;
    public float grounddrag;
    public float airdrag;

    public bool Grounded;
    public LayerMask Ground; 

    private CapsuleCollider playerCollider;
    private Rigidbody playerRb;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        playerRb = GetComponent<Rigidbody>();
        playerRb.drag = grounddrag;
    }

    // Update is called once per frame
    void Update()
    {

        Grounded = Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y, playerCollider.bounds.center.z), 0.18f, Ground);

        if (Grounded)
        {
            playerRb.drag = grounddrag;
            playerRb.mass = 1.0f;


        }
        else
        {
            playerRb.drag = airdrag;
            playerRb.mass = 50.0f;

        }
        PlayerAnim.SetBool("Grounded", Grounded);
        running = Input.GetButton("Run");
     
        //facing and aiming
        if (Input.mousePosition.x < (Screen.width / 2))
        {
            transform.localScale = flip;
            facingRight = false;

        }
        else
        {
            transform.localScale = Vector3.one;
            facingRight = true;
        }
        Vector3 pos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        MainArmIK.transform.position = pos;


    }

    private void FixedUpdate()
    {


        //movement 
        float inputLR = Input.GetAxis("Horizontal");
        if (inputLR != 0)
        {
            if (running)
            {

                PlayerAnim.SetBool("Walking", false);
                PlayerAnim.SetBool("Running", true);
                //transform.position += Vector3.right * inputLR * Time.deltaTime * runSpeed;
                playerRb.AddForce(Vector3.right * inputLR * Time.deltaTime * runSpeed, ForceMode.Force);

            }
            else
            {
                PlayerAnim.SetBool("Walking", true);
                PlayerAnim.SetBool("Running", false);
                playerRb.AddForce(Vector3.right * inputLR * Time.deltaTime * walkSpeed, ForceMode.Force);

                //transform.position += Vector3.right * inputLR * Time.deltaTime * walkSpeed;
            }

        }
        else
        {
            PlayerAnim.SetBool("Running", false);
            PlayerAnim.SetBool("Walking", false);

        }
        //jumping
        if (Input.GetButtonDown("Jump") && Grounded)
        {
            Vector3 jump = new Vector3(0, 1, 0);
            Debug.Log("jump");
            playerRb.AddForce(jump * jumpHeight, ForceMode.Impulse);

        }

    }
}
