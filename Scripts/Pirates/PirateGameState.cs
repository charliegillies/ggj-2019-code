using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PirateGameState : MonoBehaviour
{
    public static event System.Action GameLost;
    private const int NumLives = 4;

    private int m_remainingLives = NumLives;
    private int m_killCount = 0;

    [Header("State")]
    [SerializeField] private Sprite[] m_stateSprites;
    [SerializeField] private Environment m_environment;

    [Header("Chest")]
    [SerializeField] private Transform m_chestPivot;
    [SerializeField] private Vector3 m_openChestRotation;
    [SerializeField] private ParticleSystem m_chestExplosion;

    [Header("UI")]
    [SerializeField] private Image m_barImg;
    [SerializeField] private CanvasGroup m_uiGroup;
    [SerializeField] private CanvasGroup m_gameOverGroup;
    [SerializeField] private TextMeshProUGUI m_finalKd;
    [SerializeField] private float m_fadeTime = 0.5f;

    private void Start() {
        m_remainingLives = NumLives;
        PirateBehaviour.ReachedGold += OnPirateReachedGold;
        PirateBehaviour.Destroyed += OnPirateDeath;
        StartCoroutine(FadeGameScreenIn());
    }
    private void OnDestroy() {
        PirateBehaviour.ReachedGold -= OnPirateReachedGold;
        PirateBehaviour.Destroyed -= OnPirateDeath;
    }
    private void OnPirateDeath(PirateBehaviour pirate) {
        m_killCount++;
    }
    private void OnPirateReachedGold(PirateBehaviour pirate) {
        if (m_remainingLives < 0) return;

        m_remainingLives--;
        if(m_remainingLives == 0) {
            // Game is lost! All ye abandon all hope.
            Debug.Log("The game is over.");
            GameLost?.Invoke();
            m_chestExplosion.Play();
            StartCoroutine(DoGameOverSequence());
        }

        // Pivot the chest open, change the bar sprite

        float life_t = 1.0f - ((float)m_remainingLives / (float)NumLives);
        m_chestPivot.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, m_openChestRotation, life_t));
        m_barImg.sprite = m_stateSprites[m_remainingLives];
    }
    private IEnumerator FadeGameScreenIn() {
        float time = 0.0f;
        while (time < m_fadeTime) {
            time += Time.deltaTime;
            m_uiGroup.alpha = time / m_fadeTime;
            yield return null;
        }
    }
    private IEnumerator DoGameOverSequence() {
        yield return new WaitForSeconds(1.0f);

        m_gameOverGroup.gameObject.SetActive(true);
        m_finalKd.text = m_killCount.ToString();

        float time = 0.0f;
        while (time < m_fadeTime) {
            time += Time.deltaTime;
            m_gameOverGroup.alpha = time / m_fadeTime;
            m_uiGroup.alpha = 1.0f - m_gameOverGroup.alpha;
            yield return null;
        }

        // Wait for input .. 
        while (!Input.GetButtonDown("Fire1")) {
            yield return null;
        }

        time = 0.0f;
        while (time < m_fadeTime) {
            time += Time.deltaTime;
            m_gameOverGroup.alpha = 1.0f - time / m_fadeTime;
            yield return null;
        }

        m_environment.Finish();
    }
}
