using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Projectile : MonoBehaviour
{
    protected float speed = 7.0f;
    [SerializeField] ObjectPool<GameObject> ownerPool;
    [SerializeField] protected Rigidbody rb;

    [SerializeField] protected float lifeTime = 3.0f;

     protected float currentLifeTime;
    [SerializeField] protected bool penetrateObjects = true;
    [SerializeField] private float explodeRadius = 0;
    [SerializeField] private int explodeDamge = 2;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private GameObject mainProjectile;
    [SerializeField] private float destroyTime = 2;

    protected DamageInfo damageInfo;

    void Start()
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
        if(explodeRadius!=0)
        {
           Collider[] hits= Physics.OverlapSphere(transform.position,explodeRadius);
            foreach(Collider hit in hits)
            {
                Health health = hit.GetComponent<Health>();
                if (health && health.TeamID != damageInfo.caster.TeamID)
                {
                    DamageInfo damage = new DamageInfo(damageInfo.caster,
                        explodeDamge,damageInfo.Force,
                        health.transform.position-this.transform.position
                        ,damageInfo.stunTime);
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
        speed = 0;
        mainProjectile.SetActive(false);
        GetComponentInChildren<Collider>().enabled = false;

        yield return new WaitForSeconds(destroyTime);

        gameObject.SetActive(false);
        ownerPool.Release(gameObject);
    }

    protected void ResetProjectile()
    {
        mainProjectile.SetActive(true);
        hitEffect.gameObject.SetActive(false);
        GetComponentInChildren<Collider>().enabled = true;
        currentLifeTime = lifeTime;
    }

    public void SetPool(ObjectPool<GameObject> owner)
    {
        this.ownerPool = owner;
    }
}
