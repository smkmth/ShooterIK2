using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider),typeof(Rigidbody))]
public class Player : Character
{

    public Camera playerCam;
    public Animator PlayerAnim;

    private DDOL gameManager;

    public CapsuleCollider playerCollider;

    private LayerMask Ground;
    private LayerMask WeaponHit;

    public UIManager uiManager;

    private AudioSource playerAudioSource;

    [Header("Movement")]
    public float BaseWalkSpeed;
    private float walkSpeed;
    public float BaseRunSpeed;
    private float runSpeed;
    public float maxSpeed;

    private float inputLR;

    private Vector3 flip = new Vector3(-.1f, .1f, .1f);
    private Vector3 notflipped = new Vector3(.1f, .1f, .1f);

    [ReadOnly]
    public bool Grounded;
    [ReadOnly]
    public bool NearLedge;
    [ReadOnly]
    public bool facingRight = true;
    [ReadOnly]
    public bool walking = false;

    [Header("Jumping")]
    public float jumpHeight;
    public float jumpWidth;
    public float airSpeed;
    private int takenJumpActions =0;
    public int MaxJumpActions;

    [Header("Wall Jumping")]
    public float wallJumpHeight;
    public float wallJumpWidth;
    private Vector3 wallJumpVector;
    [ReadOnly]
    public bool touchingWall = false;

    [Header("Dodge")]
    public float dodgeDistance;
    public float airDodgeDistance;


    [Header("Stamina")]
    public float currentstamina;
    public float MaxStamina;
    public float RollStaminaLoss;
    public float KickStaminaLoss;
    public float StaminaRegainRate;
    public float JumpStaminaLoss;

    [Header("Gun")]
    public float fireDelay;
    private float fireTimer;
    public int gunDamage;
    public Vector3 gunKnockback;
    public AudioClip gunshotAudio;

    [Header("Kick")]
    public int KickDamage;
    public float KickTime;
    public Vector3 KickVector;


    [Header("Refs to Body Parts")]
    public HitDetect Foot;
    public GameObject MainArmIK;
    public GameObject gun;
    public GameObject playerShoulder;

    public GameObject particleObject;
    private ParticleSystem gunParticle;
    private ParticleSystem gunFlashParticle;





    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        playerCollider = GetComponent<CapsuleCollider>();
        gameManager = GetComponentInParent<DDOL>();

        uiManager = gameManager.GetComponentInChildren<UIManager>();
        currentstamina = MaxStamina;


        Ground = gameManager.whatIsGround;
        WeaponHit = gameManager.whatIsEnemy;


        playerAudioSource= GetComponent<AudioSource>();
        playerAudioSource.clip = gunshotAudio;

        gameManager.lastCheckpoint = transform.position;

        gunParticle = particleObject.GetComponent<ParticleSystem>();
        gunFlashParticle = particleObject.GetComponentInChildren<ParticleSystem>();


        Foot.isActive = false;
        Foot.force = KickVector;
        Foot.damage = KickDamage;
    }


    // Update is called once per frame
    void Update()
    {
        inputLR = Input.GetAxis("Horizontal");

        //grounded
        Vector3 modbounds = new Vector3(playerCollider.bounds.center.x , playerCollider.bounds.min.y, playerCollider.bounds.center.z); 
        Grounded = Physics.CheckSphere(modbounds, 0.03f, Ground);

        NearLedge = Physics.CheckSphere(transform.position, .2f ,LayerMask.NameToLayer("Corner"));

        if (Grounded)
        {
            takenJumpActions = 0;
            walkSpeed = BaseWalkSpeed;
            runSpeed = BaseRunSpeed;
        }
        else
        {
            walkSpeed = airSpeed;
            runSpeed = airSpeed;
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

        //shooting
        if (fireTimer <= 0.0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //particles
                if (facingRight)
                {
      
                    Vector3 rot = playerShoulder.transform.rotation.eulerAngles;
                    rot.z += 180;
        
                    Quaternion rot180degrees = Quaternion.Euler(rot);
                  
                    particleObject.transform.rotation = rot180degrees;
                }
                else
                {
                    particleObject.transform.rotation = playerShoulder.transform.rotation;


                }
                gunParticle.Emit(1);
                gunFlashParticle.Play();
                playerAudioSource.Play();


                RaycastHit hit;
                Physics.Raycast(gun.transform.position, dir * 1000.0f, out hit, 1000.0f, WeaponHit);
                Debug.DrawRay(gun.transform.position, dir * 1000.0f, Color.yellow, 1.0f);
                
                if (hit.transform != null)
                {
                    if (hit.transform.gameObject.GetComponent<HitDetect>())
                    {
                        if (hit.transform.position.x > transform.position.x)
                        {
                            gunKnockback.x = Mathf.Abs(gunKnockback.x);
                        }
                        else
                        {
                            gunKnockback.x = Mathf.Abs(gunKnockback.x)* -1;
                        }
                        hit.transform.gameObject.GetComponent<HitDetect>().DoDamageToParent(gunDamage, gunKnockback);
                    }
                }
                fireTimer = fireDelay;
            }
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }

        if (canMove)
        {
            //recover stamina over time
            if (currentstamina < MaxStamina)
            {
                currentstamina += Time.deltaTime * StaminaRegainRate;
            }


            //dodge
            if (Input.GetMouseButtonDown(1))
            {
                if (takenJumpActions <=MaxJumpActions)
                {
                    if (currentstamina > RollStaminaLoss)
                    {
                        takenJumpActions +=1;
                        StopCoroutine(Dodge());
                        StartCoroutine(Dodge());
                    }
                }
            }
            //jumping
            if (Input.GetButtonDown("Jump"))
            {
                //normal jump and double jump
                if (Grounded || takenJumpActions <= (MaxJumpActions-1))
                {
                    //calculate stamina loss only on double jumps 
                    if(takenJumpActions != 0)
                    {
                        currentstamina -= JumpStaminaLoss;

                    }
                    takenJumpActions += 1;
                    
                    //if player is standing still, just jump up, else, add some distance to the jump
                    Vector3 jump = new Vector3(0, 1, 0);
                    if (inputLR != 0)
                    {
                        if (inputLR > 0)
                        {

                            jump.x *= jumpWidth;
                        }
                        else
                        {
                            jump.x *= -jumpWidth;
                        }

                    }
                    rb.AddForce(jump * jumpHeight, ForceMode.VelocityChange);
                }

            }
            //kick
            if (Input.GetButtonDown("Kick"))
            {
                if (currentstamina > KickStaminaLoss)
                {
                    StopCoroutine(Kick());
                    StartCoroutine(Kick());
                }
            }

            //walljump - get walljump vector from normal of collision with wall
            //on on collisionenter below
            if (Input.GetButtonDown("Jump") && touchingWall && !Grounded)
            {

                Debug.Log("Wall Jump");
                wallJumpVector.Normalize();
                wallJumpVector.y += wallJumpHeight;
                if (wallJumpVector.x > 0)
                {
                    wallJumpVector.x += wallJumpWidth;

                }
                else
                {
                    wallJumpVector.x -= wallJumpWidth;

                }
                //Debug.Log(wallJumpVector * wallJumpHeight);

                rb.AddForce(wallJumpVector * wallJumpHeight, ForceMode.Impulse);
                PlayerAnim.SetTrigger("WallJump");


            }
        }
    }
    //these coroutines are for anything which needs a specfic timer, like
    //dodging or kicking. 


    IEnumerator Dodge()
    {
        thisHitDetect.isActive = false;
        canMove = false;
        currentstamina -= RollStaminaLoss;
        PlayerAnim.SetTrigger("Roll");
        Vector3 dodge = new Vector3(1, 0, 0);
        //respect keyboard movement first, else use facing direction second.
        if (inputLR > 0)
        {
            dodge.x *= 2;
        }
        else if (inputLR < 0)
        {
            dodge.x *= -2;
        }
        else
        {
            if (facingRight)
            {
                dodge.x *= 2;

            }
            else
            {
                dodge.x *= -2;

            }
        }
        if (Grounded)
        {
            rb.AddForce(dodge * dodgeDistance, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(dodge * airDodgeDistance, ForceMode.Impulse);


        }
        yield return new WaitForSeconds(.2f);
        canMove = true;
        yield return new WaitForSeconds(1f);
        thisHitDetect.isActive = true;
    }


    IEnumerator Kick()
    {
        currentstamina -= KickStaminaLoss;
        Foot.isActive = true;
        canMove = false;
        PlayerAnim.SetTrigger("Kick");
        yield return new WaitForSeconds(KickTime);
        Foot.isActive = false;
        canMove = true;
    }

  

    private void FixedUpdate()
    {
        //movement 
        if (canMove)
        {
            if (inputLR != 0)
            {
                { 
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

 

    public void SetNewCheckpoint(Vector3 checkpoint)
    {
        checkpoint.z = 0;
        gameManager.lastCheckpoint = checkpoint;
        Debug.Log("Made it to checkpoint");

    }

    public override void KillCharacter()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //rb.AddForce(Vector3.zero, ForceMode.VelocityChange);
        gameManager.ResetPlayer();
        gameManager.RestartGame();
    }

    /*
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Vector3 modbounds = new Vector3(playerCollider.bounds.center.x , playerCollider.bounds.min.y, playerCollider.bounds.center.z); 
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(modbounds, 0.03f);

       // Gizmos.color = Color.blue;
       // Gizmos.DrawSphere(transform.position, .2f);
    }
    */
}
