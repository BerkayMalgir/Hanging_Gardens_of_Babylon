using System.Collections;
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
    public float gravity = -12f;
    
    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    private bool isSliding = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!PlayerData.isGameStarted || PlayerData.gameOver)
            return;
        
        forwardSpeed = Mathf.Min(forwardSpeed + 0.1f * Time.deltaTime, maxSpeed);

        direction.z = forwardSpeed;

        HandleLaneChange();
        HandleJump();
        HandleSlide();

        ApplyGravity();
        MovePlayer();
    }

    void FixedUpdate()
    {
        if (!PlayerData.isGameStarted || PlayerData.gameOver)
            return;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.20f, groundLayer);
        direction.z = forwardSpeed;
        controller.Move(direction * Time.deltaTime);
    }

    private void HandleLaneChange()
    {
        if (SwipeManager.swipeRight || Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane = Mathf.Min(desiredLane + 1, 2);
        }
        if (SwipeManager.swipeLeft || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane = Mathf.Max(desiredLane - 1, 0);
        }
    }

    private void HandleJump()
    {
        if (isGrounded && (SwipeManager.swipeUp || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Jump();
        }
    }

    private void HandleSlide()
    {
        if (!isSliding && (SwipeManager.swipeDown || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            StartCoroutine(Slide());
        }
    }

    private void ApplyGravity()
    {
        if (!isSliding)
        {
            direction.y += gravity * Time.deltaTime;
        }
    }

    private void MovePlayer()
    {
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
            targetPosition += Vector3.left * laneDistance;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * laneDistance;

        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 30 * Time.deltaTime;
        controller.Move(moveDir.sqrMagnitude < diff.magnitude ? moveDir : diff);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Obstacle"))
        {
            PlayerData.gameOver = true;
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        float slideDuration = 0.5f;
        float slideTimer = 0f;

        Vector3 originalCenter = controller.center;
        controller.height = 0.9f;
        Vector3 originalPosition = transform.position;

        while (slideTimer < slideDuration)
        {
            slideTimer += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, originalPosition.y - 0.45f, transform.position.z);
            yield return null;
        }

        controller.height = 1.85f;
        transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
        isSliding = false;
    }
}
