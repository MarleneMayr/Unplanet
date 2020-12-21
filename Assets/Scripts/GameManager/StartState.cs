public class StartState : State
{
    public override void AfterActivate()
    {
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
