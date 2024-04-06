using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1;
    public float laneDistance = 4;
    public float jumpForce;
    public float Gravity = -12f;
    
    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public

        // Start is called before the first frame update
        void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
        
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;
        
        if (forwardSpeed < maxSpeed)
          forwardSpeed += 0.1f * Time.deltaTime;

        direction.z = forwardSpeed;

        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        if (isGrounded)
        {
            direction.y = -1;
            if (SwipeManager.swipeUp) 
            {
                Jump();
            }
        }
        
        if (SwipeManager.swipeDown)
            StartCoroutine(Slide());
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
            targetPosition += Vector3.left * laneDistance;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * laneDistance;

        //transform.position = targetPosition;
        if (transform.position != targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 30 * Time.deltaTime;
            if (moveDir.sqrMagnitude < diff.magnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }

    }

    private void FixedUpdate()
    {
        
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;
        controller.Move(direction * Time.deltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }
    private IEnumerator Slide()
    {
        
        yield return new WaitForSeconds(0.25f/ Time.timeScale);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;
        

        controller.center = Vector3.zero;
        controller.height = 2;

    }
}

