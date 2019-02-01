using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorQuitPrompt : MonoBehaviour
{
    public static event System.Action InteractBegin;
    public static event System.Action InteractEnd;

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<CharacterController>() != null)
            InteractBegin?.Invoke();
    }
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<CharacterController>() != null)
            InteractEnd?.Invoke();
    }
}
