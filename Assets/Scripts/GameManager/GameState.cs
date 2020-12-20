using DG.Tweening;
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
    [SerializeField] private ScreenEffect[] effects;
    [SerializeField] private VisualHints visualHints;
    [SerializeField] private float lightSeconds;
    [SerializeField] private float maxDistance;
    public AnimationCurve progressCurve;

    private Vector3 currentGoalPos;
    /// <summary>index of the current goal position</summary>
    public static int index { get; private set; }
    /// <summary>linear distance in the range [0..1]</summary>
    public static float distance { get; private set; }

    /// <summary>curve-influenced distance in the range [0..1]</summary>
    public static float progress { get; private set; }

    public static IntEvent FoundGoal = new IntEvent();
    private bool kinematic;

    public override void AfterActivate()
    {
        Spawn(spawnPoint);
        currentGoalPos = goal.Spawn(goalLocations[0]);
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
        distance = CalculateDistance();
        if (!kinematic) progress = progressCurve.Evaluate(distance);
    }

    private float CalculateDistance()
    {
        float d = Vector3.Distance(currentGoalPos, player.transform.position);
        return Mathf.InverseLerp(0, maxDistance, d);
    }

    private void Spawn(Transform spawnPoint)
    {
        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    private void ReachedGoal()
    {
        foreach (var e in effects)
        {
            e?.Deactivate();
        }

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
            StartCoroutine(Euphoria(lightSeconds, 2));
        }
    }

    private IEnumerator Euphoria(float seconds, float fadeIn)
    {
        yield return new WaitForSeconds(seconds);

        // spawn new
        currentGoalPos = goal.Spawn(goalLocations[index]);
        effects[index]?.Activate();

        // restart env effects
        kinematic = true;
        float targetProgress = progressCurve.Evaluate(CalculateDistance());
        DOTween.To(() => progress, x => progress = x, targetProgress, fadeIn);
        yield return new WaitForSeconds(fadeIn);

        kinematic = false;
    }
}
