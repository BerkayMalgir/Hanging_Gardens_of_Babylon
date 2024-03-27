using UnityEngine;

public enum Side { Left, Mid, Right }

public class Character : MonoBehaviour
{
    public Side currentSide = Side.Mid;

    [SerializeField] private float dodgeSpeed = 5f;
    [SerializeField] private float xValue = 3f;
    [SerializeField] private float jumpPower = 7f;
    [SerializeField] private float runSpeed = 5f; // Yeni eklendi

    private CharacterController characterController;
    private Animator animator;

    private float newXPos = 0f;
    private float yVelocity;

    private const float rollDuration = 0.2f;
    private float rollCounter;

    private float originalColHeight;
    private float originalColCenterY;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        originalColHeight = characterController.height;
        originalColCenterY = characterController.center.y;

        transform.position = Vector3.zero;
    }

    private void Update()
    {
        HandleInput();
        Move();
        Jump();
        //Roll();
        Run(); 
    }

     private void HandleInput()
    {
        bool swipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        bool swipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        bool swipeUp = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool swipeDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);

        if (swipeLeft)
        {
            if (currentSide == Side.Mid)
            {
                newXPos = -xValue;
                currentSide = Side.Left;
               // animator.Play("DodgeLeft");
            }
            else if (currentSide == Side.Right)
            {
                newXPos = 0;
                currentSide = Side.Mid;
              //  animator.Play("DodgeLeft");
            }
           // animator.Play("Run");
        }
        else if (swipeRight)
        {
            if (currentSide == Side.Mid)
            {
                newXPos = xValue;
                currentSide = Side.Right;
                //animator.Play("DodgeRight");
            }
            else if (currentSide == Side.Left)
            {
                newXPos = 0;
                currentSide = Side.Mid;
                //animator.Play("DodgeRight");
            }
           // animator.Play("Run");
        }
    }

    private void Move()
    {
        float newX = Mathf.Lerp(transform.position.x, newXPos, Time.deltaTime * dodgeSpeed);
        Vector3 moveVector = new Vector3(newX - transform.position.x, 0, 0);
        characterController.Move(moveVector);
    }

    private void Jump()
    {
        if (characterController.isGrounded)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Landing"))
            {
                //animator.Play("Landing");
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                yVelocity = jumpPower;
               // animator.CrossFadeInFixedTime("Jump", 0.1f);
            }
            else
            {
                yVelocity -= jumpPower * 2 * Time.deltaTime;
                if (characterController.velocity.y < -0.1f)
                {
                    //animator.Play("Falling");
                }
            }
        }

        characterController.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

    private void Roll()
    {
        rollCounter -= Time.deltaTime;
        if (rollCounter <= 0f)
        {
            rollCounter = 0f;
            ResetCollider();
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            rollCounter = rollDuration;
            yVelocity = -10f;
            characterController.center = new Vector3(0, originalColCenterY / 2f, 0);
            characterController.height = originalColHeight / 2f;
            animator.CrossFadeInFixedTime("roll", 0.1f);
        }
    }

    private void ResetCollider()
    {
        characterController.center = new Vector3(0, originalColCenterY, 0);
        characterController.height = originalColHeight;
    }


    private void Run()
    {
        float moveSpeed = runSpeed * Time.deltaTime;
        characterController.Move(transform.forward * moveSpeed);
    }
}
