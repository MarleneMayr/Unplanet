using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    public UnityEvent OnReached;

    private void OnTriggerEnter(Collider other)
    {
        print("hit");

        print(other.GetComponent<CharacterController>());
        print(other.gameObject.GetComponent<CharacterController>());
        if (other.GetComponent<CharacterController>())
            OnReached?.Invoke();
    }
}
