using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 direction;

    [SerializeField]  private float speed=7.0f;

    [SerializeField]  Rigidbody rb;

    [SerializeField]  float lifeTime = 3.0f;

    [SerializeField]  private bool penetrateObjects=true;
    [SerializeField] private bool destroyOnDealDamge = true;

    DamageInfo damageInfo;

    public void Initialize(DamageInfo damageInfo,Vector3 castPoint, Vector3 targetPosition)
    {
        this.damageInfo = damageInfo;
        this.direction = targetPosition - castPoint;
        this.direction=this.direction.normalized;
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
        Health health = collision.GetComponent<Health>();
        if (health)
        {
            if (damageInfo.caster.TeamID == health.TeamID)
                return;
            health.TakeDamage(damageInfo);
            Explode();
            if (destroyOnDealDamge)
                Destroy(gameObject);
        }
        else if(!collision.isTrigger)
        {
            if (penetrateObjects)
                return;
            Explode();
            Destroy(gameObject);
        }
    }
    private void Explode()
    {
 
    }
}
