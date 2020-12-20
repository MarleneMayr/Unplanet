using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalArea : MonoBehaviour
{
    public static UnityEvent PlayerInArea = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        print("Player is near Goal");
        if (other.GetComponent<CharacterController>())
        {
            PlayerInArea?.Invoke();

            // turn on basic effects here
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            // turn off basic effects here
        }
    }
}
