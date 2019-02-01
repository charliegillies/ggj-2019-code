using System;
using UnityEngine;

public class PlayerBox : MonoBehaviour
{
    public static Action InteractionBegins;
    public static Action InteractionEnds;

    private void OnTriggerEnter(Collider other) {
        InteractionBegins?.Invoke();
    }
    private void OnTriggerExit(Collider other) {
        InteractionEnds?.Invoke();
    }
}
