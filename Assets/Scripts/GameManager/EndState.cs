using UI;
using UnityEngine;

public class EndState : State
{
    public override void AfterActivate()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Menu.OnStartClicked.AddListener(StartGame);
    }

    public override void BeforeDeactivate()
    {
        Menu.OnStartClicked.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        stateMachine.GoTo<GameState>();
    }
}
