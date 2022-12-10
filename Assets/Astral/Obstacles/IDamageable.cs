namespace Astral.Obstacles
{
    public interface IDamageable
    {
        void TakeDamage(int count);
        void Kill();
    }
}