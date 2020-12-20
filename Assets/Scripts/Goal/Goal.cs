using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    public UnityEvent OnReached;
    [SerializeField] private GoalArea triggerArea;
    [SerializeField] private GoalObject goalObject;

    private void Awake()
    {
        GoalObject.PlayerInGoal.AddListener(Reached);
    }

    public Vector3 Spawn(Level.GoalLocation t)
    {
        StopEffects();
        transform.SetPositionAndRotation(t.pos, t.rot);
        triggerArea.GetComponent<Collider>().enabled = true;
        goalObject.GetComponent<Collider>().enabled = true;

        return t.pos;
    }

    public void StopEffects()
    {
        // turn off goalobject effects
        // turn off goalarea effects
    }

    public void Reached()
    {
        triggerArea.GetComponent<Collider>().enabled = false;
        goalObject.GetComponent<Collider>().enabled = false;
        OnReached?.Invoke();

        // start found effects here
    }
}
