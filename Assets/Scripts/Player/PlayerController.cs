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

    private bool isSliding = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;
        
        forwardSpeed = Mathf.Min(forwardSpeed + 0.1f * Time.deltaTime, maxSpeed);

        direction.z = forwardSpeed;

        if (SwipeManager.swipeRight || Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        if (SwipeManager.swipeLeft || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        if (isGrounded && (SwipeManager.swipeUp || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Jump();
        }

        if (!isSliding && (SwipeManager.swipeDown || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            StartCoroutine(Slide());
        }

        if (!isSliding)
        {
            direction.y += Gravity * Time.deltaTime;
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
            targetPosition += Vector3.left * laneDistance;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * laneDistance;

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
    void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;

        // Yerde mi kontrol ediliyor
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.20f, groundLayer);

        // Yönü sabitle
        direction.z = forwardSpeed;
    
        // Hareketi uygula
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
        isSliding = true;
        float slideDuration = 0.5f; // Sürgü işleminin süresi (saniye cinsinden)
        float slideTimer = 0f;

        // Karakterin orijinal collider boyutunu kaydet
        Vector3 originalCenter = controller.center;

        // Yeni collider yüksekliğini ayarla
        controller.height = 0.9f;

        Vector3 originalPosition = transform.position; // Karakterin orijinal pozisyonunu kaydet

        // Belirtilen süre boyunca bu durumda kal
        while (slideTimer < slideDuration)
        {
            slideTimer += Time.deltaTime;

            // Karakterin pozisyonunu yukarı ayarlayın
            transform.position = new Vector3(transform.position.x, originalPosition.y - 0.45f, transform.position.z);

            yield return null;
        }

        // Süre dolduktan sonra collider'ı eski haline getir
        controller.height = 1.85f;

        // Karakterin pozisyonunu eski haline getir
        transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);

        isSliding = false;
    }
}
