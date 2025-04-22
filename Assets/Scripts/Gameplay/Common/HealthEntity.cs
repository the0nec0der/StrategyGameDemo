using System;

using UnityEngine;

public abstract class HealthEntity : MonoBehaviour
{
    private float maxHP;
    private float currentHP;

    public float MaxHP => maxHP;
    public float CurrentHP
    {
        get => currentHP;
        set
        {
            float clamped = Mathf.Clamp(value, 0, maxHP);
            if (Mathf.Approximately(currentHP, clamped)) return;

            float delta = clamped - currentHP;
            currentHP = clamped;
            OnHealthChanged?.Invoke(this, delta);

            if (currentHP <= 0)
                Elimination();
        }
    }

    public bool IsActiveEntity => currentHP > 0;
    public float HPPercent => currentHP / maxHP;

    public event Action<HealthEntity, float> OnHealthChanged;
    public event Action<HealthEntity> OnDeath;

    protected void InitializeMaxHP(float hp)
    {
        maxHP = Mathf.Max(1f, hp);
        currentHP = maxHP;
    }

    public virtual void TakeDamage(float amount)
    {
        if (!IsActiveEntity || !CanTakeDamage()) return;
        CurrentHP -= amount;
    }

    public virtual void Heal(float amount)
    {
        if (!IsActiveEntity) return;
        CurrentHP += amount;
    }

    public virtual void Revive() => CurrentHP = maxHP;

    protected virtual bool CanTakeDamage() => true;

    protected virtual void Elimination()
    {
        OnDeath?.Invoke(this);
    }
}
