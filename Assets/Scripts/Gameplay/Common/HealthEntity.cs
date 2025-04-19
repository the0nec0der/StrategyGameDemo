using System;

using UnityEngine;

public abstract class HealthEntity : MonoBehaviour
{
    private int maxHP;
    private int currentHP;

    public int MaxHP => maxHP;
    public int CurrentHP
    {
        get => currentHP;
        set
        {
            if (value > maxHP)
                throw new Exception($"HP can't be higher than MaxHP ({maxHP})");

            if (currentHP == value) return;

            int delta = value - currentHP;
            currentHP = value;
            OnHealthChanged?.Invoke(this, delta);

            if (currentHP <= 0)
                Elimination();
        }
    }

    public bool IsActiveEntity => currentHP > 0;
    public event Action<HealthEntity, int> OnHealthChanged;

    protected void InitializeMaxHP(int hp)
    {
        maxHP = hp;
        CurrentHP = hp;
    }

    public virtual void TakeDamage(int amount)
    {
        if (!IsActiveEntity) return;
        CurrentHP -= amount;
    }

    protected virtual void Elimination() { }
}
