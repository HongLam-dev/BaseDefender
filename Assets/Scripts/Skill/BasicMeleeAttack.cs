using UnityEngine;

[CreateAssetMenu( fileName = "NewSkill",
    menuName = "Game/Skills/MeleAttack"
)]
class BasicMeleeAttack : SkillBase
{
    override public void ExcuteStage(SkillCastData castData, int state)
    {    
        Collider[] hits = Physics.OverlapSphere(castData.caster.transform.position, castRange);
        Vector3 direction = castData.targetPosition - castData.caster.transform.position;
        foreach (Collider collider in hits)
        {
            Health health = collider.GetComponent<Health>();
            if (health)
            {
                Vector3 hitDirection = collider.transform.position - castData.caster.transform.position;
                if (Vector3.Dot(direction,hitDirection)>=0)
                    health.TakeDamage(CalculateDamgeInfo(castData));
            }
        }
    }

    protected override int CalculateDamage(int atk)
    {
        return baseDamage + atk;
    }
}