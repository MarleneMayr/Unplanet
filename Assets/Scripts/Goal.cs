using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    public UnityEvent OnReached;

    private void OnTriggerEnter(Collider other)
    {
        print("Player reached Goal");
        if (other.GetComponent<CharacterController>())
        {
            StartEffects();
            OnReached?.Invoke();
        }
    }

    private void StartEffects()
    {

    }

    public void StopEffects()
    {

    }
}
