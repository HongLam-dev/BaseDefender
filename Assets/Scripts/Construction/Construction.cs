using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Construction : MonoBehaviour
{
    [SerializeField] private float destroyEffectTime=1;
    [SerializeField] private ParticleSystem destroyEffect;
    protected Health health;
    protected CharacterStats stats;
    virtual protected void Start()
    {
        health = GetComponent<Health>();
        stats = GetComponent<CharacterStats>();
        health.OnDeath += OnDestruction;
        health.MaxHealth = stats.MaxHealth;
        health.Defense = stats.Defense;
        health.TeamID = stats.TeamID;
    }

    virtual protected void Update()
    {
        
    }

    private void OnDestruction()
    {
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        destroyEffect.gameObject.SetActive(true);
        destroyEffect.Clear();
        destroyEffect.Play();
        yield return new WaitForSeconds(destroyEffectTime);
        Destroy(gameObject);
    }
}
