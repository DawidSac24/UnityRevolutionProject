using _Project.Scripts.Core;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int health { get; set; }
    public int maxHealth { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage(int ammount)
    {
    }
}