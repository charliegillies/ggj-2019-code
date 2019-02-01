using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomWall : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private Vector3 m_downRotation;
    [SerializeField] private float m_delay;
    [SerializeField] private float m_time;
    private Quaternion m_startRotation;
    private Quaternion m_desiredRotation;

    private void Start() {
        m_startRotation = transform.rotation;
        m_desiredRotation = Quaternion.Euler(m_downRotation);
    }
    
    public JobHandle FoldDown() {
        var handle = new JobHandle();
        StartCoroutine(DoFold(m_startRotation, m_desiredRotation, handle));
        return handle;
    }
    public JobHandle FoldUp() {
        var handle = new JobHandle();
        StartCoroutine(DoFold(m_desiredRotation, m_startRotation, handle));
        return handle;
    }
    private IEnumerator DoFold(Quaternion start, Quaternion target, JobHandle handle) {
        yield return new WaitForSeconds(m_delay);

        float time = 0.0f;
        while (time < m_time) {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / m_time);
            transform.rotation = Quaternion.Slerp(start, target, t);
            yield return null;
        }

        handle.Complete();
    }

}
