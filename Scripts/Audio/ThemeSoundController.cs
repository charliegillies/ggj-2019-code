using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource m_source;
    [SerializeField] private AudioEvent m_default;
    [SerializeField] private float m_fadeTime = 0.5f;

    private void Start() {
        Environment.Loaded += Environment_Loaded;
        Environment.Finished += Environment_Unloaded;
        StartCoroutine(SwitchAudio(m_default));
    }
    private void OnDestroy() {
        Environment.Loaded -= Environment_Loaded;
    }
    private void Environment_Loaded(Environment obj) {
        AudioEvent theme = obj.ThemeAudio;
        StartCoroutine(SwitchAudio(theme));
    }
    private void Environment_Unloaded(Environment e) {
        StartCoroutine(SwitchAudio(m_default));
    }

    private IEnumerator SwitchAudio(AudioEvent e) {
        float time = 0.0f;
        if (m_source.isPlaying) {
            // Fade audio out
            while (time < m_fadeTime) {
                time += Time.deltaTime;
                m_source.volume = 1.0f - Mathf.Clamp01(time / m_fadeTime);
                yield return null;
            }
        }

        if (e != null) {
            // Play new audio, fade in 
            e.Play(m_source);
            time = 0.0f;
            while (time < m_fadeTime) {
                time += Time.deltaTime;
                m_source.volume = Mathf.Clamp01(time / m_fadeTime);
                yield return null;
            }
        }
    }

}
