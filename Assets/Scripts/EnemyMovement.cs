using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] bool movingRightElseLeft = true;

    Rigidbody2D myRigidBody;
    Collider2D myWallDetector;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myWallDetector = GetComponent<BoxCollider2D>();

        startMovingInDirection(movingRightElseLeft);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startMovingInDirection(bool rightElseLeft)
    {
        movingRightElseLeft = rightElseLeft;
        float horizontalSpeed = moveSpeed * (movingRightElseLeft ? 1 : -1);
        myRigidBody.velocity = new Vector2(horizontalSpeed, 0);
        transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (myWallDetector.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            startMovingInDirection(!movingRightElseLeft);
        }
    }
}
