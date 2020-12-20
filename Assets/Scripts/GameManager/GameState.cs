using System.Collections;
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
    [SerializeField] private Transform[] goalLocations;
    [SerializeField] private float lightSeconds;

    public static int goalCount => index;
    private static int index = 0;

    public override void AfterActivate()
    {
        Spawn(spawnPoint);
        player.gameObject.SetActive(true);
        UIcam.gameObject.SetActive(false);
        menu.SetText(index.ToString());

        goal.OnReached.AddListener(ReachedGoal);
    }

    public override void BeforeDeactivate()
    {
        player.gameObject.SetActive(false);
        UIcam.gameObject.SetActive(true);

        menu.Hide();
        pauseMenu.Hide();
        goal.OnReached.RemoveListener(ReachedGoal);
    }

    //int steps = 10;
    //private void Update()
    //{
    //    steps--;
    //    if (steps < 0)
    //    {
    //        steps = 10;
    //        menu.SetText("FPS: " + 1f / Time.unscaledDeltaTime);
    //    }
    //}

    private void Spawn(Transform spawnPoint)
    {
        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    private void SpawnGoal(int index)
    {
        goal.transform.SetPositionAndRotation(goalLocations[index].position, goalLocations[index].rotation);
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

    private void ReachedGoal()
    {
        index++;
        if (index == goalLocations.Length)
        {
            index = 0;
            stateMachine.GoTo<EndState>();
            return;
        }
        else
        {
            menu.SetText(index.ToString());
            StartCoroutine(Light(lightSeconds));
        }
    }

    private IEnumerator Light(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        goal.StopEffects();
        SpawnGoal(index);

        // restart env effects
    }
}
