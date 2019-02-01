using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CampfireEnemy : MonoBehaviour {
    private const float MaxRandSoundTimer = 6.0f;
    private const float MinRandSoundTimer = 1.0f;

    private Camera cam;
    [SerializeField] private CharacterController player;
    [SerializeField] private float distanceForEnemyToGrabPlayer = 1.5f;
    [SerializeField] private AIManager aiManager;
    [SerializeField] private AudioEvent swipe;
    [SerializeField] private AudioEvent growl;

    private bool playedGrowl = false;
    private bool swiped = false;
    private Animator animator;
    private float timer = 0.0f;

    private enum State {
        Blink, Idle, Snarl, Attack
    };
    private State m_animState = State.Idle;
    private float m_soundTimer = 0.0f;

    private void Start() {
        cam = CameraController.Instance.GetCamera();
        gameObject.transform.rotation = cam.transform.rotation;
        animator = GetComponent<Animator>();
        m_soundTimer = Random.Range(MinRandSoundTimer, MaxRandSoundTimer);
    }
    private void Update() {
        State currentAnimState = m_animState;

        // Check distance between Player
        if ((player.transform.position - transform.position).sqrMagnitude < distanceForEnemyToGrabPlayer && (!swiped)) {
            AudioController.Instance.PlaySfx(swipe);
            swiped = true;
            Debug.Log("Attack");
            currentAnimState = State.Attack;
            aiManager.startEndGame();
        }
        m_soundTimer -= Time.deltaTime;
        if (m_soundTimer <= 0.0f) {
            m_soundTimer = Random.Range(MinRandSoundTimer, MaxRandSoundTimer);
            AudioController.Instance.PlaySfx(growl);
        }

        // https://gamedev.stackexchange.com/questions/9607/moving-an-object-in-a-circular-path // Moving Object in a circular motion
        gameObject.transform.rotation = cam.transform.rotation;
        // Check for changes in the state of the animation
        if (currentAnimState != m_animState) {
            switch (currentAnimState) {
                case State.Blink:
                    animator.Play("Blink");
                    currentAnimState = State.Blink;
                    break;

                case State.Idle:
                    animator.Play("Idle");
                    currentAnimState = State.Idle;
                    break;

                case State.Snarl:
                    animator.Play("Snarl");
                    currentAnimState = State.Snarl;
                    break;

                case State.Attack:
                    animator.Play("Scratch");
                    currentAnimState = State.Attack;
                    break;
            }
        }


    }
}
