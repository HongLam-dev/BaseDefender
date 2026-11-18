using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] Button basicAttackButton;
    [SerializeField] FixedJoystick[] skillJoysticks;
    [SerializeField] FixedJoystick movementJoystick;

    [SerializeField] PlayerController player;

    private void Start()
    {
        basicAttackButton.onClick.AddListener(player.BasicAttack);
        movementJoystick.OnJoystickDrag += player.OnMove;
        movementJoystick.OnRelease += player.OnIdle;
    }

}
