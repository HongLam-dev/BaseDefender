
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.STP;

public class DirectionProjectile : Projectile
{
    private Vector3 direction;
    private bool destroyOnDealDamge = true;
    private List<Collider> damagedTarget = new();

    public void Initialize(ProjectileConfig config, DamageInfo damageInfo, Vector3 direction,bool destroyOnHit)
    {
        damagedTarget.Clear();

        this.direction = direction.normalized;
        this.destroyOnDealDamge = destroyOnHit;
        transform.rotation = Quaternion.LookRotation(direction);

        base.Initialize(config,damageInfo);
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
        else if (!collision.isTrigger&&!config.penetrateObjects)
        {
            Explode();
            StartCoroutine(DestroyCoroutine());
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direction * config.speed;
    }
}
