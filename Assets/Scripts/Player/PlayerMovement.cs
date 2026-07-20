using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private GameObject playerObj;
    [SerializeField] private Rigidbody2D playerRigidbody2D;

    [Header("Movement Settings")]
    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private float moveDuration;
    [SerializeField] private float moveStrength;
    private bool isMoving = false;
    private float moveTime = 0f;

    private bool isFacingRight = true;

    [Header("Jumping Settings")]
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float jumpStrength;
    [SerializeField] private bool canDoubleJump = true;
    private bool isJumping = false;
    private float jumpTime = 0f;

    [Header("Wall Jumping Settings")]
    [SerializeField] private float wallJumpLockTime = 0.15f;
    [SerializeField] private float wallEjectX;
    [SerializeField] private float wallEjectY;
    private bool isWallJumping = false;
    private float wallJumperTimer = 0f;

    [Header("Ground / Wall Checks")]
    [SerializeField] private LayerMask levelLayerMask;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private GameObject wallCheck;
    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private bool isWallSliding = false;

    [Header("Knock Back Settings")]
    [SerializeField] private float knockBackDuration;
    [SerializeField] float knockBackStrength;
    [SerializeField] private bool isKnockedBack = false;

    [Header("Input System")]
    private PlayerInputs playerInputs;
    private InputAction moveAction;
    private InputAction jumpAction;

    private float xMovement;
    private float yMovement;

    private void Awake()
    {
        playerInputs = new PlayerInputs();

        playerObj = gameObject;
        playerRigidbody2D = playerObj.GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnEnable()
    {
        moveAction = playerInputs.Player.Move;
        moveAction.Enable();
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        jumpAction = playerInputs.Player.Jump;
        jumpAction.Enable();
        jumpAction.performed += OnJump;
        jumpAction.canceled += OnJump;
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

        jumpAction?.Disable();
        jumpAction.performed -= OnJump;
        jumpAction.canceled -= OnJump;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        xMovement = context.ReadValue<float>();

        if (context.performed)
        {
            if (!isMoving)
            {
                moveTime = 0f;
            }

            isMoving = Mathf.Abs(xMovement) > 0.1f;
        }

        if (context.canceled)
        {
            isMoving = false;
            moveTime = 0f;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        yMovement = context.ReadValue<float>();

        if (context.performed)
        {
            if (isGrounded)
            {
                isJumping = true;
                jumpTime = 0f;
                canDoubleJump = true;
            }
            else if (isWallSliding)
            {
                isWallJumping = true;
                wallJumperTimer = 0f;
                canDoubleJump = true;

                if (isFacingRight)
                {
                    playerRigidbody2D.linearVelocity = new Vector2(-wallEjectX, wallEjectY);
                    //playerRigidbody2D.AddForce(new Vector2(-1, 2) * jumpStrentgh);
                }
                else
                {
                    playerRigidbody2D.linearVelocity = new Vector2(wallEjectX, wallEjectY);
                    //playerRigidbody2D.AddForce(new Vector2(1, 2) * jumpStrentgh);
                }
            }
            else if (canDoubleJump)
            {
                isJumping = true;
                jumpTime = 0f;
                canDoubleJump = false;
            }
        }
        if (context.canceled)
        {
            isJumping = false;
        }
    }

    private void FixedUpdate()
    {
        // Grounded & Wall Sliding Checks
        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, groundCheckSize, 0f, levelLayerMask);
        isWallSliding = !isGrounded && Physics2D.OverlapBox(wallCheck.transform.position, wallCheckSize, 0f, levelLayerMask);

        if (isWallJumping)
        {
            wallJumperTimer += Time.fixedDeltaTime;

            if (wallJumperTimer >= wallJumpLockTime)
            {
                isWallJumping = false;
            }
        }

        if (isMoving && !isWallJumping && !isKnockedBack)
        {
            Move();
        }

        if (isJumping && !isKnockedBack)
        {
            Jump();
        }

        if (playerRigidbody2D.linearVelocityX >= 0.1f)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (playerRigidbody2D.linearVelocityX <= -0.1f)
        {
            isFacingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Move()
    {
        moveTime += Time.fixedDeltaTime;

        float t = moveTime / moveDuration;
        t = Mathf.Clamp01(t);

        float force = moveCurve.Evaluate(t);

        playerRigidbody2D.linearVelocity = new Vector2(xMovement * force * moveStrength, playerRigidbody2D.linearVelocity.y);
    }

    private void Jump()
    {
        jumpTime += Time.fixedDeltaTime;

        float t = jumpTime / jumpDuration;
        t = Mathf.Clamp01(t);

        float force = jumpCurve.Evaluate(t);

        playerRigidbody2D.linearVelocity = new Vector2(playerRigidbody2D.linearVelocity.x, force * jumpStrength);

        if (t >= 1f)
        {
            isJumping = false;
        }
    }

    private IEnumerator Knockback(float knockBackDuration, float knockBackStrength, Vector2 knockBackDirection)
    {
        isKnockedBack = true;

        playerRigidbody2D.linearVelocity = knockBackDirection * knockBackStrength;

        yield return new WaitForSeconds(knockBackDuration);

        isKnockedBack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Void"))
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            StartCoroutine(Knockback(knockBackDuration, knockBackStrength, direction));
        }
        if (collision.CompareTag("Coin"))
        {
            GameMaster.Instance.AddToScore(1);
        }
        if (collision.CompareTag("xyzPart"))
        {
            GameMaster.Instance.AddToXYZ(1);
        }
    }

    /* Debuging */
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(groundCheck.transform.position, new Vector3(groundCheckSize.x, groundCheckSize.y, 1f));
        Gizmos.DrawWireCube(wallCheck.transform.position, new Vector3(wallCheckSize.x, wallCheckSize.y, 1f));
    }
}



