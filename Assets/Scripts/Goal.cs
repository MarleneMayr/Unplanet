using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Goal : MonoBehaviour
{
    public UnityEvent OnReached;
    private Collider trigger;

    private void Awake()
    {
        trigger = GetComponent<Collider>();
    }

    public Vector3 Spawn(Transform t)
    {
        StopEffects();
        transform.SetPositionAndRotation(t.position, t.rotation);
        trigger.enabled = true;
        return t.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Player reached Goal");
        if (other.GetComponent<CharacterController>())
        {
            StartEffects();
            OnReached?.Invoke();
            trigger.enabled = false;
        }
    }

    private void StartEffects()
    {

    }

    public void StopEffects()
    {

    }
}
