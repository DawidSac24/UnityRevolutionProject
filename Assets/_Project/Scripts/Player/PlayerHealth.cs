using _Project.Scripts.Core;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float health { get; set; }
    public float maxHealth { get; set; }
    
    public void TakeDamage(float amount, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    // TODO: Handle Player death and respawn
    private void Die()
    {
        
    }
}