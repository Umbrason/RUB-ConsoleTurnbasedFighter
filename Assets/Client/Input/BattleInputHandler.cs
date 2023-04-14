using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BattleInputHandler : MonoBehaviour, BattleInput.IBattleCommandsActions
{
    public PokemonBattle battle;
    public BattleInput inputActions;
    public Button[] moveButtons;
    public Button[] switchButtons;

    void Start()
    {
        inputActions = new();
        inputActions.BattleCommands.SetCallbacks(this);
        inputActions.Enable();
        for (int i = 0; i < 4; i++)
        {
            var moveID = i + 1;
            moveButtons[i].onClick.AddListener(() => battle.MakeMove(1, moveID));
        }

        for (int i = 0; i < 6; i++)
        {
            var switchID = i + 1;
            switchButtons[i].onClick.AddListener(() => battle.Switch(1, switchID));
        }
    }

    public void OnMove1(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        battle.MakeMove(1, 1);
    }

    public void OnMove2(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        battle.MakeMove(1, 2);
    }

    public void OnMove3(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        battle.MakeMove(1, 3);
    }

    public void OnMove4(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        battle.MakeMove(1, 4);
    }

    public void OnSwitch1(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        battle.Switch(1, 1);
    }

    public void OnSwitch2(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        battle.Switch(1, 2);
    }

    public void OnSwitch3(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        battle.Switch(1, 3);
    }

    public void OnSwitch4(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        battle.Switch(1, 4);
    }

    public void OnSwitch5(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        battle.Switch(1, 5);
    }

    public void OnSwitch6(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        battle.Switch(1, 6);
    }
}
