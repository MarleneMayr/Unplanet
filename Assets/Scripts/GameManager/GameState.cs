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
    [SerializeField] private VisualHints visualHints;
    [SerializeField] private float lightSeconds;
    [SerializeField] private float maxDistance;

    public static int index { get; private set; }
    public static float progress { get; private set; }
    private Vector3 currentGoalPos;

    public static IntEvent FoundGoal = new IntEvent();

    public override void AfterActivate()
    {
        Spawn(spawnPoint);
        SpawnGoal(0);
        player.gameObject.SetActive(true);
        UIcam.gameObject.SetActive(false);
        menu.SetText(index.ToString());
        visualHints.Activate();

        goal.OnReached.AddListener(ReachedGoal);
    }

    public override void BeforeDeactivate()
    {
        player.gameObject.SetActive(false);
        UIcam.gameObject.SetActive(true);
        visualHints.Deactivate();

        menu.Hide();
        pauseMenu.Hide();

        goal.OnReached.RemoveListener(ReachedGoal);
    }

    private void Update()
    {
        float distance = Vector3.Distance(currentGoalPos, player.transform.position);
        progress = Mathf.InverseLerp(0, maxDistance, distance);
    }

    private void Spawn(Transform spawnPoint)
    {
        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    private void SpawnGoal(int index)
    {
        goal.StopEffects();
        goal.transform.SetPositionAndRotation(goalLocations[index].position, goalLocations[index].rotation);
        currentGoalPos = goalLocations[index].position;
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
        FoundGoal?.Invoke(index);
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

        SpawnGoal(index);

        // restart env effects
    }
}
