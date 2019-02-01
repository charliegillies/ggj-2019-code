using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_particles;
    [SerializeField] private Vector3 m_scalePop = new Vector3(0.8f, 0.8f, 0.8f);
    [SerializeField] private Vector3 m_axisRotation = new Vector3(0, 0, 0);
    [SerializeField] private float m_defaultRotationSpeed = 0.25f;
    [SerializeField] private float m_fastRotationSpeed = 0.5f;
    [SerializeField] private AudioEvent m_pickedAudio;

    public List<Star> Neighbours = new List<Star>();

    private bool m_playEnter = false;
    private bool m_selected = false;
    private bool m_wasPicked = false;

    private void Start() {
        PlayerStarController.StarPicked += PlayerStarController_StarPicked;
        StarGameState.BeginWave += StarGameState_BeginWave;
    }
    private void OnDestroy() {
        PlayerStarController.StarPicked -= PlayerStarController_StarPicked;
        StarGameState.BeginWave -= StarGameState_BeginWave;
    }
    private void StarGameState_BeginWave(int obj) {
        m_selected = false;
    }
    private void PlayerStarController_StarPicked(Star obj) {
        // (We don't want to unselect ones that are already selected)
        if(!m_selected) {
            m_selected = this == obj;

        }
    }
    public void PlayAudioAndParticles() {
        m_particles.Play();
        AudioController.Instance.PlaySfx(m_pickedAudio);
    }

    private void Update() {
        float rotSpeed = m_selected ? m_fastRotationSpeed : m_defaultRotationSpeed;
        transform.Rotate(m_axisRotation, rotSpeed);
    }

    public void RunPickEffect(float delay) {
        StartCoroutine(DoPickEffect(delay));
    }
    private IEnumerator DoPickEffect(float delay) {
        yield return new WaitForSeconds(delay);

        m_particles.Play();
        AudioController.Instance.PlaySfx(m_pickedAudio);

        float time = 0.0f;
        float transition = 0.5f;
        while(time < transition) {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / transition);
            transform.localScale = Vector3.Lerp(Vector3.one, m_scalePop, t);
            yield return null;
        }

        time = 0.0f;
        while (time < transition) {
            time += Time.deltaTime;
            float t = 1.0f - Mathf.Clamp01(time / transition);
            transform.localScale = Vector3.Lerp(Vector3.one, m_scalePop, t);
            yield return null;
        }
    }
}
