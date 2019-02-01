using System.Collections;
using UnityEngine;

public class JobHandle {
    private bool m_complete = false;

    public void Complete() {
        m_complete = true;
    }

    public bool IsComplete() {
        return m_complete;
    }
    
    public IEnumerator Wait() {
        yield return new WaitUntil(() => m_complete);
    }
}
