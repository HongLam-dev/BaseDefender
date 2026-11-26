

using UnityEngine;

public class HomingProjectile:Projectile
{
    Transform target;
    public void Initialize(DamageInfo damageInfo,Transform target, float speed)
    {
        ResetProjectile();

        this.target = target;
        this.damageInfo = damageInfo;
        this.speed = speed;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Health health = collision.GetComponent<Health>();
        if (health)
        {
            if (damageInfo.caster.TeamID == health.TeamID)
                return;
            health.TakeDamage(damageInfo);
            Explode();
            StartCoroutine(DestroyCoroutine());
        }
        else if (!collision.isTrigger && !penetrateObjects)
        {
            Debug.Log(collision.name);
            Explode();
            StartCoroutine(DestroyCoroutine());
        }
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;
        Vector3 direction = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angle + 180f, 0f);

        rb.linearVelocity =direction * speed;
    }
}

