public class StartState : State
{
    public override void AfterActivate()
    {
        Menu.OnStartClicked.AddListener(StartGame);
        audioManager.FadeIn(AudioManager.GlobalSound.StartMenu, 1f);
    }

    public override void BeforeDeactivate()
    {
        Menu.OnStartClicked.RemoveListener(StartGame);
        audioManager.FadeOut(AudioManager.GlobalSound.StartMenu, 0.1f);
    }

    private void StartGame()
    {
        stateMachine.GoTo<GameState>();
    }
}
