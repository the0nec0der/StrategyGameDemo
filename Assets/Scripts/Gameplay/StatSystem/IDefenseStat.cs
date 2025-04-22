namespace Gameplay.StatSystem
{
    public interface IDefenseStat
    {
        float Armor { get; }              // Reduces incoming damage
        float Resistance { get; }         // General damage reduction % (0–1)
    }
}
