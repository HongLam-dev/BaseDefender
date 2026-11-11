using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] Button basicAttackButton;
    [SerializeField] FixedJoystick[] skillJoysticks;
    [SerializeField] FixedJoystick movementJoystick;

    [SerializeField] Transform player;

    PlayerCombat combat;

    private void Start()
    {
        combat = player.GetComponent<PlayerCombat>();

        basicAttackButton.onClick.AddListener(combat.BasicAttack);
    }

}
