using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider),typeof(Rigidbody))]
public class Player : Enemy
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
    public bool canMove = true;


    private LayerMask Ground; 
    private LayerMask WeaponHit;

    public CapsuleCollider playerCollider;
   // private Rigidbody rb;
    private DDOL gameManager;

    public GameObject particleO;
    public GameObject playerShoulder;

    public ParticleSystem gunParticle;
    public ParticleSystem gunFlashParticle;
    public Vector3 Knockback;
    public AudioClip gunshot;
    AudioSource pAudioSource;

  

   // [HideInInspector]
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
        rb = GetComponent<Rigidbody>();
        gameManager = GetComponentInParent<DDOL>();


        Ground = gameManager.whatIsGround;
        WeaponHit = gameManager.whatIsEnemy;
        rb.drag = grounddrag;


        pAudioSource= gameObject.AddComponent<AudioSource>();
        pAudioSource.clip = gunshot;

        Health = 100;
        gameManager.lastCheckpoint = transform.position;

    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Vector3 modbounds = new Vector3(playerCollider.bounds.center.x , playerCollider.bounds.min.y, playerCollider.bounds.center.z); 
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(modbounds, 0.03f);
    }

    // Update is called once per frame
    void Update()
    {


        Vector3 modbounds = new Vector3(playerCollider.bounds.center.x , playerCollider.bounds.min.y, playerCollider.bounds.center.z); 

        //Grounded = Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y, playerCollider.bounds.center.z), 0.2f, Ground);
        //Debug.DrawLine(playerCollider.bounds.center * -0.5f, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y, playerCollider.bounds.center.z), Color.red, .1f) ;

        Grounded = Physics.CheckSphere(modbounds, 0.03f, Ground);
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
                if (facingRight)
                {
      
                    Vector3 rot = playerShoulder.transform.rotation.eulerAngles;
                    rot.z += 180;
        
                    Quaternion rot180degrees = Quaternion.Euler(rot);
                  
                    particleO.transform.rotation = rot180degrees;
                }
                else
                {
                    particleO.transform.rotation = playerShoulder.transform.rotation;


                }
                RaycastHit hit;
              
                gunParticle.Emit(1);
                gunFlashParticle.Play();
                pAudioSource.Play();


                Physics.Raycast(gun.transform.position, dir * 1000.0f, out hit, 1000.0f, WeaponHit);
                Debug.DrawRay(gun.transform.position, dir * 1000.0f, Color.yellow, 1.0f);
                
                if (hit.transform != null)
                {
                    if (hit.transform.gameObject.GetComponent<HitDetect>())
                    {
             
                        if (hit.transform.position.x > transform.position.x)
                        {
                            Knockback.x = Mathf.Abs(Knockback.x);
                        
                        }
                        else
                        {
                            Knockback.x = Mathf.Abs(Knockback.x)* -1;

                        }
                    

                        hit.transform.gameObject.GetComponent<HitDetect>().DoDamageToParent(10, Knockback);
                    }
                }
                timer = fireDelay;

            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
        if (canMove)
        {


            //jumping
            if (Input.GetButtonDown("Jump") && Grounded)
            {
                Vector3 jump = new Vector3(0, 1, 0);
                if (Input.GetAxis("Horizontal") != 0)
                {
                    if (facingRight)
                    {
          
                        jump.x *= 2;
                    }
                    else
                    {
                        jump.x *= -2;
                    }

                }
                rb.AddForce(jump * jumpHeight, ForceMode.Impulse);

            }
            if (Input.GetButtonDown("Kick"))
            {
                StopCoroutine(Kick());
                StartCoroutine(Kick());
            }

            if (Input.GetButtonDown("Jump") && touchingWall && !Grounded)
            {
                if (!onLedge)
                {
                    Debug.Log("Wall Jump");
                    wallJumpVector.Normalize();
                    wallJumpVector.y = wallJumpHeight;
                    Debug.Log(wallJumpVector * wallJumpHeight);

                    rb.AddForce(wallJumpVector * wallJumpHeight, ForceMode.Impulse);
                    PlayerAnim.SetTrigger("WallJump");
                }
                else
                {
                    Debug.Log("Ledge Catch");
                    rb.AddForce(new Vector3(0.0f, 6.0f, 0), ForceMode.Impulse);

                }
            }
        }
    }

    IEnumerator Kick()
    {
        canMove = false;
        PlayerAnim.SetTrigger("Kick");
        yield return new WaitForSeconds(0.5f);
        canMove = true;
    }

  

    private void FixedUpdate()
    {


        //movement 
        float inputLR = Input.GetAxis("Horizontal");
        if (canMove)
        {

            if (inputLR != 0)
            {
                if (walking)
                {

                    PlayerAnim.SetBool("Walking", true);
                    PlayerAnim.SetBool("Running", false);
                    if (rb.velocity.magnitude < maxSpeed)
                    {
                        rb.AddForce(Vector3.right * inputLR * Time.deltaTime * walkSpeed, ForceMode.Impulse);
                    }

                }
                else
                {
                    PlayerAnim.SetBool("Walking", false);
                    PlayerAnim.SetBool("Running", true);
                    if (rb.velocity.magnitude < maxSpeed)
                    {
                        rb.AddForce(Vector3.right * inputLR * Time.deltaTime * runSpeed, ForceMode.Impulse);
                    }
                }

            }
            else
            {
                PlayerAnim.SetBool("Running", false);
                PlayerAnim.SetBool("Walking", false);

            }
        }

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
        rb.useGravity = false;
        onLedge = true;


    }
    public void OffArmColliderExit(Collider hit)
    {
        if (onLedge)
        {
            onLedge = false;
            rb.useGravity = true;

        }

    }

    public void SetNewCheckpoint(Vector3 checkpoint)
    {
        checkpoint.z = 0;
        gameManager.lastCheckpoint = checkpoint;
        Debug.Log("Made it to checkpoint");

    }

    public override void KillEnemy()
    {
        gameManager.ResetPlayer();
        gameManager.RestartGame();
    }
}
