using UnityEngine;

public enum Side { Left, Mid, Right }

public class Character : MonoBehaviour
{
    public Side currentSide = Side.Mid;

    [SerializeField] private float dodgeSpeed = 5f;
    [SerializeField] private float xValue = 3f;
    [SerializeField] private float jumpPower = 7f;
    [SerializeField] private float runSpeed = 5f;

    private CharacterController characterController;
    private Animator animator;

    private float newXPos = 0f;
    private float yVelocity;

    private float originalColHeight;
    private float originalColCenterY;

    private bool isInvincible = false;
    private float invincibleDuration = 10f;
    private float invincibleTimer = 0f;

    private bool isDoubleItem = false;
    private float doubleItemDuration = 10f;
    private float doubleItemTimer = 0f;

    private bool canRevive = false;
    private float reviveDuration = 5f;
    private float reviveTimer = 0f;

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
        Run();

        UpdateTimers();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLeft();
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            MoveRight();
    }

    private void MoveLeft()
    {
        if (currentSide == Side.Mid)
        {
            newXPos = -xValue;
            currentSide = Side.Left;
        }
        else if (currentSide == Side.Right)
        {
            newXPos = 0;
            currentSide = Side.Mid;
        }
    }

    private void MoveRight()
    {
        if (currentSide == Side.Mid)
        {
            newXPos = xValue;
            currentSide = Side.Right;
        }
        else if (currentSide == Side.Left)
        {
            newXPos = 0;
            currentSide = Side.Mid;
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
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                yVelocity = jumpPower;
            }
        }
        else
        {
            yVelocity -= jumpPower * 2 * Time.deltaTime;
        }

        characterController.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

    private void Run()
    {
        float moveSpeed = runSpeed * Time.deltaTime;
        characterController.Move(transform.forward * moveSpeed);
    }

    private void UpdateTimers()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
            }
        }

        if (isDoubleItem)
        {
            doubleItemTimer -= Time.deltaTime;
            if (doubleItemTimer <= 0f)
            {
                isDoubleItem = false;
            }
        }

        if (canRevive)
        {
            reviveTimer -= Time.deltaTime;
            if (reviveTimer <= 0f)
            {
                canRevive = false;
                GameOver();
            }
        }
    }

    public void ActivateInvincibility()
    {
        isInvincible = true;
        invincibleTimer = invincibleDuration;
    }

    public void ActivateDoubleItem()
    {
        isDoubleItem = true;
        doubleItemTimer = doubleItemDuration;
    }

    public void ActivateRevive()
    {
        canRevive = true;
        reviveTimer = reviveDuration;
        SetTransparency(0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            int itemCount = isDoubleItem ? 2 : 1;
            CollectItem(itemCount);
        }
        else if (other.CompareTag("Enemy") && !isInvincible)
        {
            if (canRevive)
            {
                canRevive = false;
                reviveTimer = 0f;
                SetTransparency(1f);
            }
            else
            {
                GameOver();
            }
        }
    }

    private void CollectItem(int count)
    {
        // Item collection logic
    }

    private void GameOver()
    {
        // Game over logic
    }

    private void SetTransparency(float alpha)
    {
        Color color = GetComponent<Renderer>().material.color;
        color.a = alpha;
        GetComponent<Renderer>().material.color = color;
    }
}
