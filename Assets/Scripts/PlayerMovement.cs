using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private float moveSpeed = 5f;
    private Vector2 moveDirection;
    private Vector2 facingDirection;
    public InputActionReference move;
    public InputActionReference attack;

    public float attackRange = 2f; // Range to detect enemies
    public LayerMask enemyLayer; // Layer mask for enemies

    // Light and heavy attack
    private float attackStartTime;

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
    }

    private void OnEnable()
    {
        attack.action.started += AttackStarted;
        attack.action.canceled += AttackEnded;
    }

    private void OnDisable()
    {
        attack.action.started -= AttackStarted;
        attack.action.canceled -= AttackEnded;
    }

    private void Attack(int damage)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, facingDirection, attackRange, enemyLayer);

        if (hit.collider != null)
        {
            Debug.Log("Enemy is hit!");
            // Get the GameObject that was hit
            GameObject hitObject = hit.collider.gameObject;

            // Example: Check if the object has an Enemy script and interact with it
            Enemy enemy = hitObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, rb.position.x); // Assume TakeDamage method exists in the Enemy script
            }
        }
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
            Debug.Log("Light attack");
            Attack(1);
        }
        else
        {
            Debug.Log("Heavy attack");
            Attack(10);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}
