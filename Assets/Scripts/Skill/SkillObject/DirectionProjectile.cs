
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class DirectionProjectile : Projectile
{
    private Vector3 direction;
    [SerializeField] protected bool destroyOnDealDamge = true;
    protected List<Collider> damagedTarget = new();
    public void Initialize(DamageInfo damageInfo, Vector3 direction, float speed)
    {
        ResetProjectile();
        damagedTarget.Clear();

        this.direction = direction.normalized;
        float angle = Mathf.Atan2(this.direction.x, this.direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angle + 180f, 0f);

        this.damageInfo = damageInfo;
        this.speed = speed;
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
        else if (!collision.isTrigger&&!penetrateObjects)
        {
            Debug.Log(collision.name);
            Explode();
            StartCoroutine(DestroyCoroutine());
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }
}
