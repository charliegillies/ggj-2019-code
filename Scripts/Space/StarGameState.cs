using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGameState : MonoBehaviour
{
    public static event Action GameOver;
    public static event Action<int> BeginWave;

    [Header("State")]
    [SerializeField] private StarFieldGenerator m_starGenerator;
    [SerializeField] private float m_effectDelay = 0.25f;
    [SerializeField] private Environment m_environment;

    [Header("Gameplay")]
    [SerializeField] private int m_sequenceCount = 3;
    private Queue<Star> m_sequenceQueue = new Queue<Star>();

    [Header("UI")]
    [SerializeField] private CanvasGroup m_uiGroup;
    [SerializeField] private TMPro.TextMeshProUGUI m_text;
    [SerializeField] private float m_fadeTime = 0.5f;
    private float m_accumulatedTime = 0.0f;

    private void Awake() {
        Environment.TransitionComplete += Environment_TransitionComplete;
        PlayerStarController.StarPicked += Star_Picked;
        PlayerStarController.Dies += PlayerStarController_Dies;
    }
    private void OnDestroy() {
        Environment.TransitionComplete -= Environment_TransitionComplete;
        PlayerStarController.StarPicked -= Star_Picked;
        PlayerStarController.Dies += PlayerStarController_Dies;
    }
    private void PlayerStarController_Dies() {
        GameOver?.Invoke();
        StartCoroutine(DoGameOverSequence());
    }
    private void Start() {
        // #HACK
        // Forces scene to begin if there is no prompt to start the transition
        if (UnityEngine.SceneManagement.SceneManager.sceneCount == 1) {
            Environment_TransitionComplete();
        }
    }
    private void Environment_TransitionComplete() {
        // OK to begin the game.
        BeginNextSequence();
    }
    private void Star_Picked(Star star) {
        if(m_sequenceQueue.Count > 0) {
            // Get the front of the queue, and compare
            if (m_sequenceQueue.Peek() == star) {
                // Success, we got the next one in the sequence. Remove it!
                m_sequenceQueue.Dequeue();
                star.PlayAudioAndParticles();
                if (m_sequenceQueue.Count == 0) {
                    // We can begin the next sequence!
                    BeginNextSequence();
                }
            }
            else {
                // the comparison failed! meaning we picked a wrong sequence.
                // that means we have lost the game. boo!
                GameOver?.Invoke();
                StartCoroutine(DoGameOverSequence());
            }
        }
    }
    private void BeginNextSequence() {
        Debug.Log("Beginning the next star sequence!");
        // Pick a random set of stars that have been generated
        // .. and then run a visual effect on each to show it!
        Star[] sequence = m_starGenerator.PickSequence(m_sequenceCount);
        for(int i = 0; i < m_sequenceCount; i++) {
            Star star = sequence[i];
            star.RunPickEffect(m_effectDelay * (float)i);
            m_sequenceQueue.Enqueue(star);
        }
        BeginWave?.Invoke(m_sequenceCount);

        // Increase the next sequence by one
        m_sequenceCount++;
    }
    private IEnumerator DoGameOverSequence() {
        yield return new WaitForSeconds(1.0f);

        m_uiGroup.gameObject.SetActive(true);
        m_text.text = Mathf.Round(m_accumulatedTime).ToString();

        float time = 0.0f;
        while (time < m_fadeTime) {
            time += Time.deltaTime;
            m_uiGroup.alpha = time / m_fadeTime;
            yield return null;
        }

        // Wait for input .. 
        while (!Input.GetButtonDown("Fire1")) {
            yield return null;
        }

        time = 0.0f;
        while (time < m_fadeTime) {
            time += Time.deltaTime;
            m_uiGroup.alpha = 1.0f - time / m_fadeTime;
            yield return null;
        }

        m_environment.Finish();
    }
}
