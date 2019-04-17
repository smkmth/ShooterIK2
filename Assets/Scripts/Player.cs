using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject MainArmIK;
    public Animator PlayerAnim;

    public Vector3 flip = new Vector3(-.1f, .1f, .1f);
    public Vector3 notflipped = new Vector3(.1f, .1f, .1f);
    public bool facingRight = true;
    public bool running = false;

    public float BaseWalkSpeed;
    private float walkSpeed;
    public float BaseRunSpeed;
    private float runSpeed;
    public float jumpHeight;
    public float airspeed;
    public float grounddrag;
    public float airdrag;

    public float maxSpeed;


    public bool Grounded;
    public LayerMask Ground; 
    public LayerMask WeaponHit;
    public GameObject gun;

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

        Grounded = Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y, playerCollider.bounds.center.z), 0.01f, Ground);

        if (Grounded)
        {
            playerRb.drag = grounddrag;
            playerRb.mass = 1.0f;
            walkSpeed = BaseWalkSpeed;
            runSpeed = BaseRunSpeed;
            
            
        }
        else
        {
            walkSpeed = airspeed;
            runSpeed = airspeed;
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
            transform.localScale = notflipped;
            facingRight = true;
        }
        Vector3 pos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        MainArmIK.transform.position = pos;

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            
            Physics.Raycast(gun.transform.position, pos, out hit, 100.0f, WeaponHit);
            Debug.DrawRay(gun.transform.position, pos, Color.yellow, 10.0f);
            if (hit.transform != null)
            {
                Debug.Log("Hit");
            }
            
           
           

        }


    }

    private void FixedUpdate()
    {


        //movement 
        float inputLR = Input.GetAxis("Horizontal");
        if (inputLR != 0 )
        {
            if (running)
            {

                PlayerAnim.SetBool("Walking", false);
                PlayerAnim.SetBool("Running", true);
                if (playerRb.velocity.magnitude < maxSpeed)
                {
                    playerRb.AddForce(Vector3.right * inputLR * Time.deltaTime * runSpeed, ForceMode.Impulse);
                }

            }
            else
            {
                PlayerAnim.SetBool("Walking", true);
                PlayerAnim.SetBool("Running", false);
                if (playerRb.velocity.magnitude < maxSpeed)
                {
                    playerRb.AddForce(Vector3.right * inputLR * Time.deltaTime * walkSpeed, ForceMode.Impulse);
                }
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
