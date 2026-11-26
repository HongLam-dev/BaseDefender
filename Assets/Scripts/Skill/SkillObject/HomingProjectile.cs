

using UnityEngine;

public class HomingProjectile:Projectile
{
    Vector3 direction;
    Transform target;
    float rotationSpeed;
    public void Initialize(ProjectileConfig config, DamageInfo damageInfo, Transform target, float rotationSpeed)
    {
        ResetProjectile();

        this.target = target;
        this.rotationSpeed = rotationSpeed;

        direction = target.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);


        base.Initialize(config,damageInfo);
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
        else if (!collision.isTrigger && !config.penetrateObjects)
        {
            Explode();
            StartCoroutine(DestroyCoroutine());
        }
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;
        direction = (target.transform.position - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        rb.MoveRotation(
            Quaternion.RotateTowards(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            )
        );

        rb.linearVelocity = transform.forward * config.speed;
    }

}

