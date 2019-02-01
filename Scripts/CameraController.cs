using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Singletons are bad! 
    // .. but they're also very useful. Sometimes.
    public static CameraController Instance { get; private set; }

    [SerializeField] private Camera m_camera;
    [SerializeField] private float m_minSize = 4.0f;
    [SerializeField] private float m_maxSize = 8.0f;

    private void Awake() {
        if(Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Assert(m_camera != null);    
    }

    public Camera GetCamera() {
        return m_camera;
    }

    public void ZoomIn() {
        m_camera.orthographicSize = m_minSize;
    }
    public void ZoomOut() {
        m_camera.orthographicSize = m_maxSize;
    }
}
