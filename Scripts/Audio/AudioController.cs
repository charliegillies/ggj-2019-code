using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    private List<AudioSource> m_sources = new List<AudioSource>();

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public AudioSource GetFreeAudioSource() {
        for(int i = 0; i < m_sources.Count; i++) {
            if (!m_sources[i].isPlaying) return m_sources[i];
        }

        var source = gameObject.AddComponent<AudioSource>();
        m_sources.Add(source);
        return source;
    }

    public void PlaySfx(AudioEvent e) {
        AudioSource source = GetFreeAudioSource();
        e.Play(source);
    }
}
