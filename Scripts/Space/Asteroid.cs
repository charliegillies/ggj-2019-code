using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Direction is constant - it won't change!
    private Vector3 m_direction;
    private Vector3 m_rotation;
    private float m_spinSpeed;
    private float m_movementSpeed;

    private void Update() {
        transform.Rotate(m_rotation, m_spinSpeed);
        transform.position += m_direction * m_movementSpeed * Time.deltaTime;

        // TODO: Geometry utility to check if we're out the frustum..
    }
    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<PlayerStarController>();
        if (player != null) {
            player.Kill();
        }
    }
    public void Begin(Vector3 target, Vector3 scale, float spinSpeed, float movementSpeed) {
        m_direction = (target - transform.position).normalized;
        transform.localScale = scale;
        m_rotation = transform.forward;
        m_spinSpeed = spinSpeed;
        m_movementSpeed = movementSpeed;
    }

}
