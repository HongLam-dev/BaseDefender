using UnityEngine;

[CreateAssetMenu( fileName = "NewSkill",
    menuName = "Game/Skills/MeleAttack"
)]
class BasicMeleeAttack : SkillBase
{
    override public void ExcuteStage(SkillCastData castData, int state)
    {    
        Collider[] hits = Physics.OverlapSphere(castData.caster.CasterTransform.position, castRange);
        foreach (Collider collider in hits)
        {
            Health health = collider.GetComponent<Health>();
            if (health)
            {
                Vector3 hitDirection = collider.transform.position - castData.caster.CasterTransform.position;
                if (Vector3.Dot(castData.caster.CasterTransform.forward,hitDirection)>=0)
                    health.TakeDamage(CalculateDamgeInfo(castData));
            }
        }
    }

    protected override int CalculateDamage(int atk)
    {
        return baseDamage + atk;
    }
}