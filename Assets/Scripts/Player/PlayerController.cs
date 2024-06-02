using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1; // 0: Left, 1: Middle, 2: Right
    public float laneDistance = 4; // The distance between two lanes
    public float jumpForce;
    public float gravity = -12f;

    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    private bool isSliding = false;

    public float laneChangeSpeed = 10f; // Speed for lane change smoothing

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
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

        // Smoothly move to the target lane position
        Vector3 moveDirection = Vector3.Lerp(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);
        controller.Move(moveDirection - transform.position);
    }

    private void Jump()
    {
        direction.y = jumpForce;
        animator.SetTrigger("Jump"); // Trigger the jump animation
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
        animator.SetTrigger("Slide"); // Trigger the slide animation

        // Save the original height and center
        float originalHeight = controller.height;
        Vector3 originalCenter = controller.center;

        // Adjust the height and center for sliding
        controller.height = originalHeight / 2;
        controller.center = new Vector3(originalCenter.x, originalCenter.y / 2, originalCenter.z);

        // Slide duration should match the length of the slide animation
        float slideDuration = 1.0f; // Adjust this value to match your slide animation duration
        yield return new WaitForSeconds(slideDuration);

        // Restore the original height and center
        controller.height = originalHeight;
        controller.center = originalCenter;

        isSliding = false;
    }
}
