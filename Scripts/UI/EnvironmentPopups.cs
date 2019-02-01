using System.Collections;
using UnityEngine;

public class EnvironmentPopups : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_piratePopup;
    [SerializeField] private CanvasGroup m_campPopup;
    [SerializeField] private CanvasGroup m_spacePopup;
    [SerializeField] private float m_fadeTime = 0.5f;

    public JobHandle ShowPiratePrompt() {
        JobHandle handle = new JobHandle();
        StartCoroutine(DoRunPrompt(m_piratePopup, handle));
        return handle;
    }
    public JobHandle ShowCampPrompt() {
        JobHandle handle = new JobHandle();
        StartCoroutine(DoRunPrompt(m_campPopup, handle));
        return handle;
    }
    public JobHandle ShowSpacePrompt() {
        JobHandle handle = new JobHandle();
        StartCoroutine(DoRunPrompt(m_spacePopup, handle));
        return handle;
    }

    private IEnumerator DoRunPrompt(CanvasGroup group, JobHandle handle) {
        group.gameObject.SetActive(true);

        float time = 0.0f;
        while(time < m_fadeTime) {
            time += Time.deltaTime;
            m_piratePopup.alpha = time / m_fadeTime;
            yield return null;
        }

        // Wait for input .. 
        while(!Input.GetButtonDown("Fire1")) {
            yield return null;
        }

        time = 0.0f;
        while (time < m_fadeTime) {
            time += Time.deltaTime;
            m_piratePopup.alpha = 1.0f - time / m_fadeTime;
            yield return null;
        }

        handle.Complete();
        group.gameObject.SetActive(false);
    }

}
