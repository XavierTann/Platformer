using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private float moveSpeed = 5f;
    private Vector2 moveDirection;
    private Vector2 facingDirection;

    [Header("Movement Settings")]
    public InputActionReference move;

    [Header("Attack Settings")]
    public InputActionReference attack;

    private float attackStartTime;
    public float attackRange = 2f; // Range to detect enemies
    public LayerMask enemyLayer; // Layer mask for enemies

    [Header("Jump Settings")]
    public InputActionReference jump;
    public float jumpForce = 500f; // Force applied for jumping

    public LayerMask groundLayer; // Layer mask for the ground
    public Transform groundCheck; // Reference to a ground check point
    public float groundCheckRadius = 0.1f; // Radius for ground check
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
        if (moveDirection != Vector2.zero)
        {
            facingDirection = moveDirection.normalized;
        }

        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    private void OnEnable()
    {
        attack.action.started += AttackStarted;
        attack.action.canceled += AttackEnded;
        jump.action.started += Jump;
    }

    private void OnDisable()
    {
        attack.action.started -= AttackStarted;
        attack.action.canceled -= AttackEnded;
        jump.action.started -= Jump;
    }

    private void AttackStarted(InputAction.CallbackContext context)
    {
        attackStartTime = Time.time;
    }

    private void AttackEnded(InputAction.CallbackContext context)
    {
        float attackDuration = Time.time - attackStartTime;
        if (attackDuration < 1f)
        {
            Attack(1);
        }
        else
        {
            Attack(10);
        }
    }

    private void Attack(int damage)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, facingDirection, attackRange, enemyLayer);

        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;

            Enemy enemy = hitObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, rb.position.x); // Assume TakeDamage method exists in the Enemy script
            }
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
        }
    }
}
