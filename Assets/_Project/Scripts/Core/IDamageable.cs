using UnityEngine;

namespace _Project.Scripts.Core
{
    public interface IDamageable
    {
        float health { get; set; }
        float maxHealth { get; set; }

        public void TakeDamage(float amount, Vector3 hitPoint, Vector3 hitNormal); 
    }
}