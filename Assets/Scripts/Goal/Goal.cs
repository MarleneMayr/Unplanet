using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class Goal : MonoBehaviour
{
    public UnityEvent OnReached;
    [SerializeField] private GoalArea triggerArea;
    [SerializeField] private GoalObject goalObject;
    [SerializeField] private VisualEffect vfx;

    private void Awake()
    {
        GoalObject.PlayerInGoal.AddListener(Reached);
    }

    public Vector3 Spawn(Level.GoalLocation t)
    {
        transform.SetPositionAndRotation(t.pos, t.rot);

        triggerArea.GetComponent<Collider>().enabled = true;
        goalObject.GetComponent<Collider>().enabled = true;

        return t.pos;
    }

    public void StopEffects()
    {
        vfx.Stop();

    }

    public void Reached()
    {
        triggerArea.GetComponent<Collider>().enabled = false;
        goalObject.GetComponent<Collider>().enabled = false;
        OnReached?.Invoke();

        vfx.Play();
    }
}
