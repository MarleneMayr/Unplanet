using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalArea : MonoBehaviour
{
    [SerializeField] private GoalObject goalObject;

    private void OnTriggerEnter(Collider other)
    {
        print("Player is near Goal");
        if (other.GetComponent<CharacterController>())
        {
            // turn on basic effects here
            goalObject.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            // turn off basic effects here
            goalObject.gameObject.SetActive(false);
        }
    }
}
