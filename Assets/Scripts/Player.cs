using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider),typeof(Rigidbody))]
public class Player : MonoBehaviour
{

    public GameObject MainArmIK;
    public GameObject gun;

    public Animator PlayerAnim;

    public float BaseWalkSpeed;
    private float walkSpeed;
    public float BaseRunSpeed;
    private float runSpeed;
    public float jumpHeight;
    public float wallJumpHeight;
    private Vector3 wallJumpVector;
    public float airspeed;
    public float maxSpeed;

    public float grounddrag;
    public float airdrag;

    public float fireDelay;
    private float timer;

    public bool touchingWall = false;
    public bool onLedge = false;


    private LayerMask Ground; 
    private LayerMask WeaponHit;

    private CapsuleCollider playerCollider;
    private SphereCollider offArmCollider;
    private Rigidbody playerRb;
    private DDOL gameManager;

    [HideInInspector]
    public bool Grounded;
    [HideInInspector]
    public bool facingRight = true;
    [HideInInspector]
    public bool running = false;

    private Vector3 flip = new Vector3(-.1f, .1f, .1f);
    private Vector3 notflipped = new Vector3(.1f, .1f, .1f);

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        playerRb = GetComponent<Rigidbody>();
        gameManager = GetComponentInParent<DDOL>();


        Ground = gameManager.whatIsGround;
        WeaponHit = gameManager.whatIsEnemy;
        playerRb.drag = grounddrag;
    }

    // Update is called once per frame
    void Update()
    {

        Grounded = Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y, playerCollider.bounds.center.z), 0.01f, Ground);

        if (Grounded)
        { 

            walkSpeed = BaseWalkSpeed;
            runSpeed = BaseRunSpeed;
        }
        else
        {
            walkSpeed = airspeed;
            runSpeed = airspeed;
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

        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(gun.transform.position);

        if (timer <= 0.0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                Physics.Raycast(gun.transform.position, dir, out hit, 1000.0f, WeaponHit);
                Debug.DrawRay(gun.transform.position, dir * 1000.0f, Color.yellow, 1.0f);
                if (hit.transform != null)
                {
                    Debug.Log("Hit");
                }
                timer = fireDelay;

            }
        }
        else
        {
            timer -= Time.deltaTime;
        }

        //jumping
        if (Input.GetButtonDown("Jump") && Grounded)
        {
            Vector3 jump = new Vector3(1, 1, 0);

            playerRb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

        }

        if (Input.GetButtonDown("Jump") && touchingWall)
        {
            if (!onLedge)
            {
                wallJumpVector.y = (jumpHeight / 2);
                Debug.Log(wallJumpVector * wallJumpHeight);

                playerRb.AddForce(wallJumpVector * wallJumpHeight, ForceMode.Impulse);
                Debug.Log("walljump");
            }
            else
            {
                playerRb.AddForce(new Vector3(0.0f, 6.0f, 0),ForceMode.Impulse);

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
    

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Grounded)
        {
            if (collision.GetContact(0).normal.x > 0.9 || collision.GetContact(0).normal.x < -0.9)
            {
                Debug.Log("Hit corner");
                touchingWall = true;
                Debug.DrawRay(collision.GetContact(0).point, collision.GetContact(0).normal, Color.red, 10.0f);

                wallJumpVector = collision.GetContact(0).normal;
            }
        }
        
    }
    private void OnCollisionExit(Collision collision)
    {
        touchingWall = false;


    }

    public void OffArmColliderHit(Collider hit)
    {
      
        Debug.Log(hit.gameObject.name + "Hit");
        playerRb.useGravity = false;
        onLedge = true;

       // playerRb.AddForce(new Vector3(0.0f, 6.0f, 0),ForceMode.Impulse);

    }
    public void OffArmColliderExit(Collider hit)
    {
        if (onLedge)
        {
            onLedge = false;

            Debug.Log(hit.gameObject.name + "Left");
            playerRb.useGravity = true;

        }

    }
}
