using UnityEngine;

public class HealingPulse : MonoBehaviour
{
    private int healAmount;
    private Health target;
    private float healInterval;
    private float timer;

    private int healTime;

    private float healedTime;

    public void Initialize(int healAmount, float healInterval,int healTime, Health target)
    {
        this.target = target;
        this.healInterval = healInterval;
        this.healAmount = healAmount;
        this.healTime = healTime;

        HealTarget();
        healedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (healedTime == healTime)
        {  
            Destroy(gameObject);
            return;
        }
        if (timer>=healInterval)
        {
            HealTarget();
        }
    }

    private void HealTarget()
    {
        healedTime++;
        target.Heal(healAmount);
        timer = 0;
    }
}
