using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] private CharacterController player;
    [SerializeField] private GameObject prompt;

    [SerializeField] private float m_dropTimer = 0.25f;
    [SerializeField] private float m_dropHeight = 2.0f;

    private bool m_running = false;
    private bool m_dropped = false;
    private Vector3 m_dropPosition;

    private void OnEnable() {
        m_dropped = false;
    }
    private void OnDisable() {
        prompt?.SetActive(false);
    }

    public void Drop() {
        gameObject.SetActive(true);
        StartCoroutine(DoDrop());
    }
    private IEnumerator DoDrop() {
        m_dropped = false;
        Vector3 pos = transform.localPosition;
        Vector3 drop = pos + new Vector3(0.0f, m_dropHeight, 0.0f);

        float time = 0.0f;
        while(time < m_dropTimer) {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / m_dropTimer);
            transform.localPosition = Vector3.Lerp(drop, pos, t);
            yield return null;
        }
        m_dropped = true;
    }

    private void OnTriggerStay(Collider other)
    {
        prompt?.SetActive(!player.isCarryingStick);

        // Still falling? Don't allow pickup
        if (!m_dropped) return;

        if (!(player.isCarryingStick)) {
            if (Input.GetButtonDown("Fire1")) {
                gameObject.SetActive(false);
                player.isCarryingStick = true;
            }
        }
    }
}
