using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private Asteroid m_asteroidPrefab;

    [Header("Behaviour")]
    [SerializeField] private float m_minFrequency = 8.0f;
    [SerializeField] private float m_maxFrequency = 15.0f;
    [SerializeField] private Vector3 m_minScale = new Vector3(0.8f, 0.8f, 0.8f);
    [SerializeField] private Vector3 m_maxScale = new Vector3(1.2f, 1.2f, 1.2f);
    [SerializeField] private float m_minSpin = 0.0f;
    [SerializeField] private float m_maxSpin = 5.0f;
    [SerializeField] private float m_minSpeed = 2.0f;
    [SerializeField] private float m_maxSpeed = 5.0f;
    [SerializeField] private Transform m_player;

    private bool m_running = false;
    private float m_nextTimer = 0.0f;

    private void Awake() {
        StarGameState.BeginWave += StarGameState_BeginWave;
        StarGameState.GameOver += StarGameState_GameOver;
        m_nextTimer = Random.Range(m_minFrequency, m_maxFrequency);
    }
    private void OnDestroy() {
        StarGameState.BeginWave -= StarGameState_BeginWave;
        StarGameState.GameOver -= StarGameState_GameOver;
    }
    private void StarGameState_BeginWave(int obj) {
        m_running = true;
    }
    private void StarGameState_GameOver() {
        m_running = false;
    }

    private void Update() {
        if (!m_running) return;

        m_nextTimer -= Time.deltaTime;
        if (m_nextTimer <= 0.0f) {
            SpawnAsteroid();
            m_nextTimer = Random.Range(m_minFrequency, m_maxFrequency);
        }
    }
    private void SpawnAsteroid() {
        var asteroid = Instantiate(m_asteroidPrefab, transform);
        asteroid.Begin(m_player.transform.position, 
            Helpers.Range(m_minScale, m_maxScale),
            Random.Range(m_minSpin, m_maxSpin),
            Random.Range(m_minSpeed, m_maxSpeed)
        );
    }



}
