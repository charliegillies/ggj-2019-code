using System;
using System.Collections;
using UnityEngine;

using URandom = UnityEngine.Random;

public class PirateBehaviour : MonoBehaviour
{
    public static event Action<PirateBehaviour> Spawned;
    public static event Action<PirateBehaviour> Destroyed;
    public static event Action<PirateBehaviour> ReachedGold;

    [Header("Behaviour")]
    [SerializeField] private PirateBehaviourConfig m_config;
    [SerializeField] private ParticleSystem m_particles;

    [Header("Sink")]
    [SerializeField] private Vector3 m_sinkRotation;

    [Header("Effect")]
    [SerializeField] private float m_amplitude = 1.0f;
    [SerializeField] private float m_period = 0.5f;

    private Vector3 m_startPosition;
    private Vector3 m_dropPosition;

    private bool m_isDropping = false;
    private bool m_isSinking = false;
    private float m_accumulatedTime;

    private float m_movementTime = 0.0f;
    private float m_interpolationTime;

    private void Awake() {
        PirateGameState.GameLost += PirateGameState_GameLost;
    }
    private void OnDestroy() {
        PirateGameState.GameLost -= PirateGameState_GameLost;
    }
    private void PirateGameState_GameLost() {
        enabled = false;
    }

    private void Start() {
        transform.localScale = Helpers.Range(new Vector3(0.9f, 0.9f, 0.9f), new Vector3(1.1f, 1.1f, 1.1f));

        m_interpolationTime = URandom.Range(m_config.MinTime, m_config.MaxTime);

        // Cache the spawn position, drop the pirate in 
        m_startPosition = transform.position;
        m_dropPosition = m_startPosition + new Vector3(0, m_config.DropHeight, 0);
        transform.position = m_dropPosition;
        StartCoroutine(DoDrop());

        transform.rotation = Quaternion.LookRotation(PirateSpawner.TreasurePosition - m_startPosition);

        // Offset time/distance for each pirate, this helps with the floating effect
        m_accumulatedTime = m_startPosition.sqrMagnitude + (URandom.value / 2.0f);

        Spawned?.Invoke(this);
    }
    private IEnumerator DoDrop() {
        // Begin drop, wait for a random amount of time
        m_isDropping = true;
        float delay = URandom.Range(m_config.DropDelayMin, m_config.DropDelayMax);
        yield return new WaitForSeconds(delay);
        
        float time = 0.0f;
        while(time < m_config.DropTime) {
            time += Time.deltaTime;
            float t = m_config.DropCurve.Evaluate(Mathf.Clamp01(time / m_config.DropTime));
            transform.position = Vector3.LerpUnclamped(m_dropPosition, m_startPosition, t);
            yield return null;
        }

        m_isDropping = false;
    }

    private void Update() {
        m_accumulatedTime += Time.deltaTime;
        if (m_isDropping || m_isSinking) return;

        // Move towards the island (0, 0, 0)
        m_movementTime += Time.deltaTime;
        float t = m_config.MovementCurve.Evaluate(Mathf.Clamp01(m_movementTime / m_interpolationTime));
        transform.position = Vector3.Lerp(m_startPosition, PirateSpawner.TreasurePosition, t);
        if (Vector3.Distance(transform.position, PirateSpawner.TreasurePosition) < m_config.TargetRange) {
            ReachedGold?.Invoke(this);
            AudioController.Instance.PlaySfx(m_config.BootyEvent);
            Destroy(gameObject);
        }

        // Animate the y axis to appear as if we're moving with the waves
        float axis = AnimateFloatAxis();
        var position = transform.localPosition;
        position.y = axis;
        transform.localPosition = position;
    }
    private float AnimateFloatAxis() {
        float theta = m_accumulatedTime / m_period;
        float distance = m_amplitude * Mathf.Sin(theta);
        return distance;
    }

    public void Sink() {
        AudioEvent e = Helpers.RandomElement(m_config.SinkAudioEvents);
        AudioController.Instance.PlaySfx(e);

        m_particles.Play();
        StartCoroutine(DoSink());
        m_isSinking = true;
    }
    private IEnumerator DoSink() {
        Vector3 pos = transform.localPosition;
        Vector3 sinkPos = pos - new Vector3(0, m_config.SinkHeight, 0);

        // Randomize sail falling left/right
        Quaternion startRot = transform.rotation;
        Vector3 randRotation = startRot.eulerAngles + new Vector3(0, 0, Helpers.RandBool() ? -50.0f : 50.0f);
        Quaternion targRot = Quaternion.Euler(randRotation);

        float time = 0.0f;
        while (time < m_config.DropTime) {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / m_config.DropTime);
            transform.rotation = Quaternion.Slerp(startRot, targRot, t);

            t = m_config.SinkCurve.Evaluate(t);
            transform.localPosition = Vector3.LerpUnclamped(pos, sinkPos, t);
            yield return null;
        }

        Destroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
