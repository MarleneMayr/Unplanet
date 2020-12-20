using UI;
using UnityEngine;

public class GameState : State
{
    [SerializeField] private Menu pauseMenu;

    [Header("GameObjects")]
    [SerializeField] private Goal goal;
    [SerializeField] private CharacterController player;
    [SerializeField] private Camera UIcam;
    [SerializeField] private Transform spawnPoint;

    public override void AfterActivate()
    {
        Spawn(spawnPoint);
        player.gameObject.SetActive(true);
        UIcam.gameObject.SetActive(false);

        goal.OnReached.AddListener(EndGame);
    }

    public override void BeforeDeactivate()
    {
        player.gameObject.SetActive(false);
        UIcam.gameObject.SetActive(true);

        menu.Hide();
        pauseMenu.Hide();
        goal.OnReached.RemoveListener(EndGame);
    }

    int steps = 10;
    private void Update()
    {
        steps--;
        if (steps < 0)
        {
            steps = 10;
            menu.SetText("FPS: " + 1f / Time.unscaledDeltaTime);
        }
    }

    private void Spawn(Transform spawnPoint)
    {
        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    private void Pause()
    {
        menu.Hide(0);
        pauseMenu.Show(0);

        Time.timeScale = 0f;
    }

    private void Unpause()
    {
        Time.timeScale = 1f;
        pauseMenu.Hide(0);
        menu.Show();
    }

    private void EndGame()
    {
        stateMachine.GoTo<EndState>();
    }
}
