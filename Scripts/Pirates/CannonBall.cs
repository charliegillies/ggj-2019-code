using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private Rigidbody m_body;
    [SerializeField] private float m_speed = 10.0f;
    [SerializeField] private float m_maxLifetime = 10.0f;

    public void Update() {
        m_maxLifetime -= Time.deltaTime;
        if(m_maxLifetime <= 0.0f) {
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector3 normDir) {
        m_body.velocity = normDir * m_speed;
    }

    private void OnCollisionEnter(Collision collision) {
        PirateBehaviour pirate = collision.gameObject?.GetComponent<PirateBehaviour>();
        if(pirate != null) {
            // Inform the pirate ship that it needs to sink
            pirate.Sink();

            // .. then destroy our own gameObject!
            Destroy(gameObject);
        }
    }
}
