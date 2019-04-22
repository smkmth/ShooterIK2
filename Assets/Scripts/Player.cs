using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider),typeof(Rigidbody))]
public class Player : MonoBehaviour
{

    public GameObject MainArmIK;
    public GameObject gun;
    public Camera playerCam;

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
    private Rigidbody playerRb;
    private DDOL gameManager;

    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            Debug.Log("hit enemy health = " + health);
            health = value;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    [HideInInspector]
    public bool Grounded;

    public bool NearLedge;
    [HideInInspector]
    public bool facingRight = true;
    [HideInInspector]
    public bool walking = false;

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

        Health = 100;
    }

    // Update is called once per frame
    void Update()
    {

        Grounded = Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y, playerCollider.bounds.center.z), 0.01f, Ground);
        NearLedge = Physics.CheckSphere(transform.position, 1f ,LayerMask.NameToLayer("Corner"));
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
        walking = Input.GetButton("Run");
     
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
        Vector3 pos = playerCam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        MainArmIK.transform.position = pos;

        Vector3 dir = Input.mousePosition - playerCam.WorldToScreenPoint(gun.transform.position);

        if (timer <= 0.0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                Physics.Raycast(gun.transform.position, dir * 1000.0f, out hit, 1000.0f, WeaponHit);
                Debug.DrawRay(gun.transform.position, dir * 1000.0f, Color.yellow, 1.0f);
                if (hit.transform != null)
                {
                    if (hit.transform.gameObject.GetComponent<Enemy>())
                    {
                        hit.transform.gameObject.GetComponent<Enemy>().Health -= 10;
                    }
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
            Vector3 jump = new Vector3(0, 1, 0);
            if (Input.GetAxis("Horizontal") != 0)
            {
                if (facingRight)
                {
                    Debug.Log("Jump right");

                    jump.x *= 2;
                }
                else
                {
                    Debug.Log("Jump Left");
                    jump.x *= -2;
                }

            }
            Debug.Log("Jump");


            playerRb.AddForce(jump * jumpHeight, ForceMode.Impulse);

        }

        if (Input.GetButtonDown("Jump") && touchingWall && !Grounded)
        {
            if (!onLedge)
            {
                Debug.Log("Wall Jump");
                wallJumpVector.Normalize();
                wallJumpVector.y = wallJumpHeight;
                Debug.Log(wallJumpVector * wallJumpHeight);

                playerRb.AddForce(wallJumpVector * wallJumpHeight, ForceMode.Impulse);
                PlayerAnim.SetTrigger("WallJump");
            }
            else
            {
                Debug.Log("Ledge Catch");
                playerRb.AddForce(new Vector3(0.0f, 6.0f, 0),ForceMode.Impulse);

            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {


        //movement 
        float inputLR = Input.GetAxis("Horizontal");
        if (inputLR != 0 )
        {
            if (walking)
            {

                PlayerAnim.SetBool("Walking", true);
                PlayerAnim.SetBool("Running", false);
                if (playerRb.velocity.magnitude < maxSpeed)
                {
                    playerRb.AddForce(Vector3.right * inputLR * Time.deltaTime * walkSpeed, ForceMode.Impulse);
                }

            }
            else
            {
                PlayerAnim.SetBool("Walking", false);
                PlayerAnim.SetBool("Running", true);
                if (playerRb.velocity.magnitude < maxSpeed)
                {
                    playerRb.AddForce(Vector3.right * inputLR * Time.deltaTime * runSpeed, ForceMode.Impulse);
                }
            }

        }
        else
        {
            PlayerAnim.SetBool("Running", false);
            PlayerAnim.SetBool("Walking", false);

        }
    

    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

    }

    public void TakeDamage(int damage, Vector3 knockback)
    {
        Health -= damage;
        playerRb.AddForce(knockback,ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
   
        if (collision.GetContact(0).normal.x > 0.9 || collision.GetContact(0).normal.x < -0.9)
        {
            touchingWall = true;
            Debug.DrawRay(collision.GetContact(0).point, collision.GetContact(0).normal, Color.red, 10.0f);

            wallJumpVector = collision.GetContact(0).normal;
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


    }
    public void OffArmColliderExit(Collider hit)
    {
        if (onLedge)
        {
            onLedge = false;
            playerRb.useGravity = true;

        }

    }
}
