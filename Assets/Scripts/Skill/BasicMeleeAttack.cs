using UnityEngine;

[CreateAssetMenu( fileName = "NewSkill",
    menuName = "Game/Skills/MeleAttack"
)]
class BasicMeleeAttack : SkillBase
{
    override public void ExcuteStage(SkillCastData castData, int state)
    {    
        Collider2D[] hits = Physics2D.OverlapCircleAll(castData.caster.transform.position, castRange);
        Vector2 direction = castData.targetPosition - castData.caster.transform.position;

        foreach (Collider2D collider in hits)
        {
            if (!collider.isTrigger)
                continue;
            Health health = collider.GetComponent<Health>();
            if (health)
            {
                Vector2 hitDirection = collider.transform.position - castData.caster.transform.position;
                if (Vector2.Dot(direction,hitDirection)>0)
                    health.TakeDamage(new DamageInfo(castData.caster.TeamID,CalculateDamage(castData.caster.Atk),force));
            }
        }
    }

    protected override int CalculateDamage(int atk)
    {
        return baseDamage + atk;
    }
}