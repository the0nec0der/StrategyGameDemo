namespace Gameplay.StatSystem
{
    public interface IDamageStat
    {
        float Damage { get; }
        float AttackSpeed { get; }
        float CriticalChance { get; }
        float CriticalMultiplier { get; }
    }
}
