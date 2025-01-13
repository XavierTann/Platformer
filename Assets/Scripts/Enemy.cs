using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private int health = 100;

    public float lightAttackKnockback = 2f; // Knockback force for light attack
    public float heavyAttackKnockback = 5f; // Knockback force for heavy attack

    public Transform player; // Reference to the player's transform
    public float followRange = 5f; // Range within which the enemy starts following
    public float speed = 2f; // Movement speed of the enemy

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (player == null)
            return;

        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the player is within follow range, move toward them
        if (distanceToPlayer <= followRange)
        {
            Debug.Log("Following player");
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
        }
        else
        {
            // Stop moving if the player is out of range
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    public void TakeDamage(int damage, float playerPositionX)
    {
        if (rb.position.x > playerPositionX)
        {
            if (damage == 1)
            {
                Debug.Log("Light attack");
                KnockBack(lightAttackKnockback);
            }
            else if (damage == 10)
            {
                Debug.Log("Heavy attack");
                KnockBack(heavyAttackKnockback);
            }
        }
        else
        {
            if (damage == 1)
            {
                Debug.Log("Light attack");
                KnockBack(-lightAttackKnockback);
            }
            else if (damage == 10)
            {
                Debug.Log("Heavy attack");
                KnockBack(-heavyAttackKnockback);
            }
        }

        health -= damage;
    }

    private void KnockBack(float knockbackForce)
    {
        // Apply a force to the Rigidbody2D in the negative X direction
        rb.AddForce(new Vector2(knockbackForce, 0), ForceMode2D.Impulse);
    }
}
