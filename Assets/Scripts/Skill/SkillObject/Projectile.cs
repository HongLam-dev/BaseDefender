using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.Rendering.STP;

public abstract class Projectile : MonoBehaviour
{
    ObjectPool<GameObject> ownerPool;
    protected Rigidbody rb;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private GameObject mainProjectile;
    [SerializeField] private float explosionEffectTime = 2;
    private float currentLifeTime;
    protected ProjectileConfig config;
    protected DamageInfo damageInfo;

    protected void Initialize(ProjectileConfig config,DamageInfo damageInfo)
    {
        ResetProjectile();

        this.config = config;
        this.damageInfo = damageInfo;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentLifeTime -= Time.deltaTime;
        if (currentLifeTime <= 0)
            StartCoroutine(DestroyCoroutine());
    }

    protected void Explode()
    {
        if(config.explodeRadius!=0)
        {
           Collider[] hits= Physics.OverlapSphere(transform.position, config.explodeRadius);
            foreach(Collider hit in hits)
            {
                Health health = hit.GetComponent<Health>();
                if (health && health.TeamID != damageInfo.caster.TeamID)
                {
                    DamageInfo damage = new DamageInfo(damageInfo.caster,
                        config.explodeDamage, damageInfo.Force,
                        health.transform.position-this.transform.position
                        , damageInfo.stunTime);
                    health.TakeDamage(damage);
                }
            }
        }
        if(hitEffect)
        {
            hitEffect.Clear();
            hitEffect.Play();
            hitEffect.gameObject.SetActive(true);
        }
    }

    protected IEnumerator DestroyCoroutine()
    {
        config.speed = 0;
        mainProjectile.SetActive(false);
        GetComponentInChildren<Collider>().enabled = false;

        yield return new WaitForSeconds(explosionEffectTime);

        gameObject.SetActive(false);
        ownerPool.Release(gameObject);
    }

    protected void ResetProjectile()
    {
        mainProjectile.SetActive(true);
        hitEffect.gameObject.SetActive(false);
        GetComponentInChildren<Collider>().enabled = true;
        currentLifeTime = config.lifeTime;
    }

    public void SetPool(ObjectPool<GameObject> owner)
    {
        this.ownerPool = owner;
    }
}
