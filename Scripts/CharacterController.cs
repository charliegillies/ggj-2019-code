using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed;
    private Vector3 forward, right;
    private Animator animator;
    [SerializeField]
    private GameObject stick;
    private Camera m_cam;

    public bool isCarryingStick = false;
    private float m_splashLock = 1.5f;

    private bool m_isLocked = false;
    private Vector3 m_lockedPos;

    public void Lock() {
        m_isLocked = true;
        m_lockedPos = transform.localPosition;
    }
    public void Unlock() {
        m_isLocked = false;
    }

    private void Start()
    {
        m_cam = CameraController.Instance.GetCamera();

        // Movement
        forward = m_cam.transform.forward;
        // No Y component.
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        // Animation
        animator = GetComponent<Animator>();
        stick.SetActive(false);
    }

    private void Update() {
        if (stick != null)
            stick.SetActive(isCarryingStick);

        m_splashLock -= Time.deltaTime;
        if (m_isLocked || m_splashLock > 0.0f) return;

        // Don't move unless any input is received.
        float axis = Input.GetAxis("Horizontal") + Input.GetAxis("Vertical");
        if (!Mathf.Approximately(axis, 0.0f))
        {
            if(Move())
                animator.SetInteger("AnimationPar", 1); // Change to Walk cycle animation
        }
        else
        {
            animator.SetInteger("AnimationPar", 0); // Change to Standing animation
        }
    }

    private bool Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        Vector3 rightVec = right * playerSpeed * Time.deltaTime * horizontal;
        Vector3 upVec = forward * playerSpeed * Time.deltaTime * vertical;
        Vector3 heading = Vector3.Normalize(rightVec + upVec);

        // Look towards the heading
        transform.forward = heading;

        // Raycast to ensure we can actually move this way
        Vector3 origin = transform.position;
        //Debug.DrawRay(origin, heading * 0.5f, Color.green);

        RaycastHit hit;
        if (!Physics.Raycast(origin, heading * 0.25f, out hit, 0.2f)) {
            transform.position += rightVec;
            transform.position += upVec;
            return true;
        }
        else {
            //Debug.DrawRay(origin, hit.point, Color.red);
        }
        return false;
    }
}
