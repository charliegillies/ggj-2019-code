using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGameState : MonoBehaviour
{
    [SerializeField] private Environment m_environment;
    [SerializeField] private CanvasGroup m_uiGroup;
    [SerializeField] private TMPro.TextMeshProUGUI m_text;
    [SerializeField] private float m_fadeTime = 0.5f;
    private float m_accumulatedTime = 0.0f;

    [SerializeField] private GameObject m_aiManager;

    private void Awake() {
        Campfire.GameOver += Campfire_GameOver;
        Environment.TransitionComplete += Environment_TransitionComplete;

        if (Helpers.ExampleMode()) {
            Environment_TransitionComplete();
        }
    }
    private void OnDestroy() {
        Campfire.GameOver -= Campfire_GameOver;
        Environment.TransitionComplete -= Environment_TransitionComplete;
    }
    private void Environment_TransitionComplete() {
        m_aiManager.gameObject.SetActive(true);
    }
    private void Update() {
        m_accumulatedTime += Time.deltaTime;
    }
    private void Campfire_GameOver() {
        StartCoroutine(DoGameOverSequence());
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
