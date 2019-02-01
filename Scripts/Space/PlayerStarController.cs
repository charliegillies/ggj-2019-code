using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStarController : MonoBehaviour
{
    public static event System.Action<Star> StarPicked;
    public static event System.Action Dies;

    [SerializeField] private Star m_star;
    [SerializeField] private bool m_isJumping = false;
    [SerializeField] private float m_arcJumpHeight = 2.5f;
    [SerializeField] private Animator m_animator;
    [SerializeField] private AudioEvent m_jumpAudio;
    [SerializeField] private AudioEvent m_landAudio;

    private bool m_gameOver = false;

    private void Start() {
        transform.position = m_star.transform.position;
        m_animator.SetInteger("AnimationPar", 0);

        StarGameState.GameOver += StarGameState_GameOver;
    }
    private void OnDestroy() {
        StarGameState.GameOver -= StarGameState_GameOver;
    }
    private void StarGameState_GameOver() {
        m_gameOver = true;
    }
    private void Update() {
        if (m_isJumping) return;
        if (m_gameOver) return;

        if (Input.GetButtonDown("Fire1")) {
            StarPicked?.Invoke(m_star);
        }

        // Movement controls.. 
        float axis = Input.GetAxis("Horizontal") + Input.GetAxis("Vertical");
        if(!Mathf.Approximately(axis, 0.0f)) {
            StopAllCoroutines();
            if (axis > 0.0f) {
                StartCoroutine(JumpTo(m_star.Neighbours[1]));
            }
            else {
                StartCoroutine(JumpTo(m_star.Neighbours[0]));
            }
        }
    }
    private IEnumerator JumpTo(Star star) {
        m_isJumping = true;
        Vector3 a = transform.position;
        Vector3 b = star.transform.position;

        // "Getting ready to jump"
        m_animator.SetInteger("AnimationPar", 2);
        Quaternion rot = transform.rotation;
        Quaternion dest = Quaternion.LookRotation(b - a);
        float time = 0.0f;
        float duration = 0.25f;
        while (time < duration) {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(rot, dest, Mathf.Clamp01(time / duration));
            yield return null;
        }

        // "Jump!"
        m_animator.SetInteger("AnimationPar", 3);

        AudioController.Instance.PlaySfx(m_jumpAudio);

        time = 0.0f;
        duration = Vector3.Distance(a, b) / 2.5f;
        while(time < duration) {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            transform.position = Helpers.JumpArc(a, b, t, m_arcJumpHeight);
            yield return null;
        }

        AudioController.Instance.PlaySfx(m_landAudio);

        // "End Jump!"
        m_animator.SetInteger("AnimationPar", 0);
        m_star = star;
        m_isJumping = false;
    }

    public void Kill() {

    }
}
