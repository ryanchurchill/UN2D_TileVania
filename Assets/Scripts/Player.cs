using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // constants
    const string ANIMATOR_PARAM_RUNNING = "Running";

    // config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;

    // state
    bool isAlive = true;

    // cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRunInput();
        HandleJumpInput();
        FlipSprite();
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
            Vector2 jumpVelocityToAdd = new Vector2(0, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
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
