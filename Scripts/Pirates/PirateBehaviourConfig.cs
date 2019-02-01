using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pirate Config")]
public class PirateBehaviourConfig : ScriptableObject
{
    [Header("Drop Animation")]
    public float DropHeight = 5.0f;
    public float DropTime = 0.5f;
    public float DropDelayMin = 0.1f;
    public float DropDelayMax = 0.3f;
    public AnimationCurve DropCurve;

    [Header("Sink Animation")]
    public float SinkHeight = 2.0f;
    public float SinkTime = 1.0f;
    public AnimationCurve SinkCurve;
    public AudioEvent[] SinkAudioEvents;
    public AudioEvent BootyEvent;

    [Header("Timings")]
    public float MaxTime = 2.5f;
    public float MinTime = 1.2f;
    public AnimationCurve MovementCurve;
    public float TargetRange = 2.5f;
}
