using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;

    [SerializeField]
    private float speed=7.0f;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    float lifeTime = 3.0f;

    [SerializeField]
    private bool destroyOnContact=true;
    DamageInfo damageInfo;

    public void Initialize(DamageInfo damageInfo,Vector2 castPoint, Vector2 targetPosition)
    {
        this.damageInfo = damageInfo;
        this.direction = targetPosition - castPoint;
        this.direction=this.direction.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();
        if (health)
        {
            if (damageInfo.TeamID == health.TeamID)
                return;
            health.TakeDamage(damageInfo);
            Explode();
        }
        else if(!collision.isTrigger)
        {
            Explode();
        }
    }
    private void Explode()
    {
        if(destroyOnContact)
            Destroy(gameObject);
    }
}
