using UnityEngine;
using UnityEngine.UI;
using System;
public class Health : MonoBehaviour
{
    [SerializeField] private int teamID=0;
    public int TeamID { set => teamID = value; get => teamID; }
    private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    private int defense = 0;
    public int Defense { set => defense = value; get => defense; }

    public int MaxHealth { get => maxHealth; set
        {
            maxHealth = value;
            currentHealth = maxHealth;
        }
    }
    public int CurrentHealth => currentHealth;

    public event Action<DamageInfo> OnHurt;
    public event Action OnDeath;
    public event Action<HealthChangeInfo> OnHealthChange;

    public bool IsDead => currentHealth<=0;

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (IsDead||this.teamID == damageInfo.caster.TeamID||currentHealth<=0)
            return;
        int damageIntake = damageInfo.Damage - defense;
        if (damageIntake <= 0)
            damageIntake = 1;
        currentHealth -=damageIntake;
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;    
            Die();
        }
        else
        {
            OnHurt?.Invoke(damageInfo);
        }
        OnHealthChange?.Invoke(new HealthChangeInfo(this, -damageIntake));
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        OnHealthChange?.Invoke(new HealthChangeInfo(this,amount));
    }

    public void Die()
    {
        OnDeath?.Invoke();
    }
    
}
