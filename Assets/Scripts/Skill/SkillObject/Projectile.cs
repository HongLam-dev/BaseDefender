using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    private Vector3 direction;

    private float speed = 7.0f;

    [SerializeField] ObjectPool<GameObject> ownerPool;
    [SerializeField] Rigidbody rb;

    [SerializeField] float lifeTime = 3.0f;
    [SerializeField] float flyheight=1.5f;
    [SerializeField] private bool penetrateObjects = true;
    [SerializeField] private bool destroyOnDealDamge = true;
    [SerializeField] private float explodeRadius = 0;
    [SerializeField] private int explodeDamge = 2;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private GameObject mainProjectile;
    [SerializeField] private float destroyTime = 2;

    List<Collider> damagedTarget=new();
    DamageInfo damageInfo;
    public void Initialize(DamageInfo damageInfo, Vector3 direction, float speed)
    {
        this.damageInfo = damageInfo;
        this.speed = speed;
        this.direction = direction.normalized;
        this.direction.y = 0;
        transform.position = new Vector3(transform.position.x, flyheight, transform.position.z);
        float angle = Mathf.Atan2(this.direction.x, this.direction.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, angle + 180f, 0f);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (damagedTarget.Contains(collision))
            return;
        Health health = collision.GetComponent<Health>();
        if (health)
        {
            if (damageInfo.caster.TeamID == health.TeamID)
                return;
            health.TakeDamage(damageInfo);
            damagedTarget.Add(collision);
            Explode();
            if (destroyOnDealDamge)
            {
                StartCoroutine(DestroyCoroutine());
            }
        }
        else if (!collision.isTrigger)
        {
            if (penetrateObjects)
                return;
            Explode();
            StartCoroutine(DestroyCoroutine());
        }
    }
    private void Explode()
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

    IEnumerator DestroyCoroutine()
    {
        speed = 0;
        mainProjectile.SetActive(false);
        GetComponentInChildren<Collider>().enabled = false;
        yield return new WaitForSeconds(destroyTime);
        gameObject.SetActive(false);
        ownerPool.Release(gameObject);
    }

    public void SetPool(ObjectPool<GameObject> owner)
    {
        this.ownerPool = owner;
    }
}
