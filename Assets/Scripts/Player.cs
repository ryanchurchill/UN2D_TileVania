using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // constants
    const string ANIMATOR_PARAM_RUNNING = "Running";
    const string ANIMATOR_PARAM_CLIMBING = "Climbing";

    // config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    float standardGravityScale;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);

    // state
    bool isAlive = true;

    // cached component references
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();

        standardGravityScale = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            HandleRunInput();
            HandleJumpInput();
            FlipSprite();
            HandleClimbInput();
        }
    }

    private void HandleRunInput()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        myAnimator.SetBool(ANIMATOR_PARAM_RUNNING, isRunning());
    }

    private void HandleJumpInput()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                Vector2 jumpVelocityToAdd = new Vector2(0, jumpSpeed);
                myRigidBody.velocity += jumpVelocityToAdd;
            }
        }
    }

    private void HandleClimbInput()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical"); // -1 to +1
        bool isClimbing = false;
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            
            Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * runSpeed);
            myRigidBody.velocity = playerVelocity;

            isClimbing = (Mathf.Abs(controlThrow) > Mathf.Epsilon);
            myRigidBody.gravityScale = 0;
        }
        else
        {
            myRigidBody.gravityScale = standardGravityScale;
        }
        myAnimator.SetBool(ANIMATOR_PARAM_CLIMBING, isClimbing);
        
    }

    private void FlipSprite()
    {
        if (isRunning())
        {
            if (isRunning())
            {
                transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y);
            }
        }
    }

    private bool isRunning()
    {
        return (Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAlive && myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard")))
        {
            Die();
        }
    }

    private void Die()
    {
        print("die");
        isAlive = false;
        myAnimator.SetTrigger("Dying");
        myRigidBody.velocity = deathKick;
    }
}
