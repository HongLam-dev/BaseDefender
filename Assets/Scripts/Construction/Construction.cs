using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Construction : MonoBehaviour
{
    [SerializeField] protected int teamID=0;
    [SerializeField] private float destroyEffectTime=1;
    [SerializeField] private ParticleSystem destroyEffect;
    [SerializeField] private int defense=50;
    [SerializeField] private int maxHP=100;

    protected Health health;
    virtual protected void Start()
    {
        health = GetComponent<Health>();
        health.TeamID = teamID;
        health.OnDeath += OnDestruction;
        health.MaxHealth = maxHP;
        health.Defense = defense;
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
