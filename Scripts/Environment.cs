using System;
using System.Collections;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public enum ID {
        Pirate,
        Campsite,
        Space
    }

    public static event Action<Environment> Loaded;
    public static event Action<Environment> Finished;
    public static event Action TransitionComplete;

    [Header("Transition")]
    [SerializeField] private Vector3 m_startPosition;
    [SerializeField] private float m_delay = 0.1f;
    [SerializeField] private float m_transitionTime = 1.0f;
    [SerializeField] private AnimationCurve m_downCurve;
    [SerializeField] private AnimationCurve m_upCurve;

    [Header("Scene management")]
    [SerializeField] private ID m_id;
    [SerializeField] private string m_sceneName = "";

    [Header("Data")]
    public AudioEvent ThemeAudio;

    private void Start() {
        // HACK:
        // Move down ONLY if we have more then one active scene
        if (UnityEngine.SceneManagement.SceneManager.sceneCount > 1) {
            transform.position = m_startPosition;
        }

        // Inform any listeners that our environment is ready to go
        Loaded?.Invoke(this);
        Debug.Assert(!string.IsNullOrEmpty(m_sceneName));
    }

    public void Unload() {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(m_sceneName);
    }

    public void Finish() {
        Finished?.Invoke(this);
    }

    public JobHandle TransitionUp() {
        var handle = new JobHandle();
        StartCoroutine(Transition(Vector3.zero, m_upCurve, handle));
        return handle;
    }
    public JobHandle TransitionDown() {
        var handle = new JobHandle();
        StartCoroutine(Transition(m_startPosition, m_downCurve, handle));
        return handle;
    }
    private IEnumerator Transition(Vector3 target, AnimationCurve curve, JobHandle handle) {
        yield return new WaitForSeconds(m_delay);

        Vector3 start = transform.position;
        float time = 0.0f;
        while(time < m_transitionTime) {
            time += Time.deltaTime;
            float t = curve.Evaluate(Mathf.Clamp01(time / m_transitionTime));
            transform.position = Vector3.LerpUnclamped(start, target, t);
            yield return null;
        }

        TransitionComplete?.Invoke();
        handle.Complete();
    }

    public ID GetID() {
        return m_id;
    }
}
