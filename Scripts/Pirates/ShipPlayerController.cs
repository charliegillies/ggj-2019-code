using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlayerController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private Transform m_pivot;
    [SerializeField] private float m_rotateSpeed;

    [Header("Shooting")]
    [SerializeField] private Transform m_origin;
    [SerializeField] private Transform m_turret;
    [SerializeField] private CannonBall m_cannonBall;
    [SerializeField] private float m_fireRate = 0.25f;
    [SerializeField] private float m_lastFire = 0.0f;
    [SerializeField] private ParticleSystem m_particles;
    [SerializeField] private AudioEvent[] m_cannonBlasts;

    private void Awake() {
        PirateGameState.GameLost += PirateGameState_GameLost;
    }
    private void OnDestroy() {
        PirateGameState.GameLost -= PirateGameState_GameLost;
    }
    private void PirateGameState_GameLost() {
        enabled = false;
    }

    private void Update() {
        float hozAxis = Input.GetAxis("Horizontal");
        if(!Mathf.Approximately(hozAxis, 0.0f)) {
            m_pivot.Rotate(Vector3.up, hozAxis * m_rotateSpeed);
        }

        m_lastFire += Time.deltaTime;
        if (m_lastFire >= m_fireRate ) {
            if (Input.GetButtonDown("Fire1")) {
                CannonBall ball = Instantiate(m_cannonBall, m_turret.position, m_turret.rotation);
                ball.Shoot((m_turret.position - m_origin.position).normalized);

                // Play the particles & sfx
                m_particles.Play();
                AudioEvent e = Helpers.RandomElement(m_cannonBlasts);
                AudioController.Instance.PlaySfx(e);

                m_lastFire = 0.0f;
            }
        }
        else {
            float scale_t = Mathf.Clamp01(m_lastFire / m_fireRate);
            m_turret.localScale = Vector3.Lerp(new Vector3(1.5f, 1.5f, 1.5f), Vector3.one, scale_t);
        }
    }
}
