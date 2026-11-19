using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] Button basicAttackButton;
    [SerializeField] Joystick[] skillJoysticks;
    [SerializeField] Joystick movementJoystick;

    [SerializeField] PlayerController player;
    private Character playerCharacter;

    private void Start()
    {
        playerCharacter = player.GetComponent<Character>();

        basicAttackButton.onClick.AddListener(player.BasicAttack);
        movementJoystick.OnJoystickDrag += player.OnMove;
        movementJoystick.OnRelease += player.OnIdle;

        playerCharacter.OnStunned += DisableInput;
        playerCharacter.OnStunRecover += EnableInput;
    }

    private void DisableInput()
    {
        movementJoystick.enabled = false;
        basicAttackButton.enabled = false;
        foreach (Joystick joystick in skillJoysticks)
        {
            joystick.enabled = false;
        }
    }

    private void EnableInput()
    {
        movementJoystick.enabled = true;
        basicAttackButton.enabled = true;
        foreach (Joystick joystick in skillJoysticks)
        {
            joystick.enabled = true;
        }
    }
}
