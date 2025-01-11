using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb; // Reference to the Rigidbody2D component
    private int health = 100;
    public float lightAttackKnockback = 2f; // Knockback force for light attack
    public float heavyAttackKnockback = 5f; // Knockback force for heavy attack

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
