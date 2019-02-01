using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundController : MonoBehaviour
{
    [SerializeField] private bool m_fadeSoundIn = true;
    [SerializeField] private bool m_fadeSoundOut = true;
    [SerializeField] private AudioSource m_source;
    [SerializeField] private AudioEvent m_ambience;

    private void Start() {
        m_ambience.Play(m_source);
    }
}
