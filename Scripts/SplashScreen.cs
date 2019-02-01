using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private float m_fadeTime = 0.5f;

    private void Start() {
        StartCoroutine(Transition());
    }
    private IEnumerator Transition() {
        yield return new WaitForSeconds(1.0f);

        m_canvasGroup.gameObject.SetActive(true);
        float time = 0.0f;
        while (time < m_fadeTime) {
            time += Time.deltaTime;
            m_canvasGroup.alpha = 1.0f - time / m_fadeTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

}
