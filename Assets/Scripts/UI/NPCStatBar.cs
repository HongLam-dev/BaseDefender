
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCStatBar : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private TMP_Text levelText;

    public void SetHealth(int currentHealth, int maxHealth)
    {
        if (maxHealth <= 0)
        {
            healthFill.fillAmount = 0f;
            return;
        }

        healthFill.fillAmount =
            Mathf.Clamp01((float)currentHealth / maxHealth);
    }

    public void SetLevel(int level)
    {
        levelText.text = level.ToString();
    }
}
