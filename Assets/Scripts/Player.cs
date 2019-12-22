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

    // state
    bool isAlive = true;

    // cached component references
    Rigidbody2D myRigidBody;
    Collider2D myCollider;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();

        standardGravityScale = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRunInput();
        HandleJumpInput();
        FlipSprite();
        HandleClimbInput();
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
            if (myCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
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
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
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
}
