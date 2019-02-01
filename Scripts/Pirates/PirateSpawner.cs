using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateSpawner : MonoBehaviour
{
    public static Vector3 TreasurePosition { get; private set; }
    public static event System.Action<int> WaveBegins;

    [Header("Spawning")]
    [SerializeField] private PirateSpawnConfiguration m_config;
    [SerializeField] private PirateBehaviour m_piratePrefab;
    [SerializeField] private float m_spawnDistance = 16.0f;

    private List<PirateBehaviour> m_activePirates = new List<PirateBehaviour>();
    [SerializeField] private Transform m_treasureTransform;

    private int m_spawnedSoFar = 0;
    private int m_numWave = 0;

    private void OnEnable() {
        PirateBehaviour.Spawned += OnPirateSpawned;
        PirateBehaviour.Destroyed += OnPirateDestroyed;
        PirateBehaviour.ReachedGold += OnPirateDestroyed;
        Environment.TransitionComplete += SpawnNextWave;
    }
    private void OnDisable() {
        PirateBehaviour.Spawned -= OnPirateSpawned;
        PirateBehaviour.Destroyed -= OnPirateDestroyed;
        PirateBehaviour.ReachedGold -= OnPirateDestroyed;
        Environment.TransitionComplete -= SpawnNextWave;
    }
    private void OnPirateSpawned(PirateBehaviour behaviour) {
        m_activePirates.Add(behaviour);
    }
    private void OnPirateDestroyed(PirateBehaviour behaviour) {
        m_activePirates.Remove(behaviour);

        if (m_activePirates.Count == 0) {
            SpawnNextWave();
        }
    }
    private void SpawnNextWave() {
        m_numWave++;

        int numPirates = 0;
        if(m_numWave == 1) {
            numPirates = m_config.InitialWaveSize;
        }
        else {
            numPirates = m_spawnedSoFar + Random.Range(m_config.WaveIncreasePerTurnMin, m_config.WaveIncreasePerTurnMax);
        }

        for(int i = 0; i < numPirates; i++) {
            SpawnPirate();
        }

        m_spawnedSoFar += numPirates;
        WaveBegins?.Invoke(numPirates);
    }

    private void Start() {
        TreasurePosition = m_treasureTransform.position;

        // HACK:
        // If we're not transitioning from the screen - don't wait for the event
        // as it will never come!
        if(UnityEngine.SceneManagement.SceneManager.sceneCount == 1) {
            SpawnNextWave();
        }
    }
    private void SpawnPirate() {
        // Spawn at a random x,z vector 
        Vector3 spawn = Helpers.RandNormalXZ().normalized * m_spawnDistance;
        PirateBehaviour pirate = Instantiate(m_piratePrefab, transform);
        pirate.transform.localPosition = spawn;
    }
}
