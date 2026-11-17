namespace _Project.Scripts.Core
{
    public interface IDamageable
    {
        int health { get; set; }
        int maxHealth { get; set; }

        public void TakeDamage(int ammount);
    }
}