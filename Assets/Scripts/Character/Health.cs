using UnityEngine;
using UnityEngine.UI;
using System;
public class Health : MonoBehaviour
{
    private int teamID=0;
    public int TeamID { set => teamID = value; get => teamID; }
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    private CharacterAnimationController ani;
    public event Action<DamageInfo> OnHurt;
    public event Action OnDie;
    public event Action<HealthChangeInfo> OnHealthChange;

    private void Start()
    {
        ani = GetComponentInChildren<CharacterAnimationController>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (this.teamID == damageInfo.TeamID||currentHealth<=0)
            return;
        currentHealth -= damageInfo.Damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;    
            Die();
        }
        else
        {
            OnHurt?.Invoke(damageInfo);
        }
        OnHealthChange?.Invoke(new HealthChangeInfo(this, -damageInfo.Damage));
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
        OnDie?.Invoke();
    }
    
}
