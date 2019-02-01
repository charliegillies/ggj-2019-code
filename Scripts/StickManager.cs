using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickManager : MonoBehaviour
{
    [SerializeField]
    private Stick[] sticks;

    // Start is called before the first frame update
    private void Start()
    {
        Random.InitState((int)(System.DateTime.Now.Ticks));
        sticks = GetComponentsInChildren<Stick>();
        // Turn off all sticks incase.
        for(int i = 0; i < sticks.Length; i++)
        {
            sticks[i].gameObject.SetActive(false);
        }

        Environment.TransitionComplete += Environment_TransitionComplete;
        if (Helpers.ExampleMode())
            Environment_TransitionComplete();
    }
    private void OnDestroy() {
        Environment.TransitionComplete -= Environment_TransitionComplete;
    }

    private void Environment_TransitionComplete() {
        SpawnMoreSticks();
    }

    // Update is called once per frame
    private void Update()
    {
        // Are no sticks seeable.
        if(!(AreAnySticksLeft()))
        {
            SpawnMoreSticks();
        }
    }

    bool AreAnySticksLeft()
    {
        for(int i = 0; i < sticks.Length; i++)
        {
            if(sticks[i].gameObject.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }

    void SpawnMoreSticks()
    {
        sticks[Random.Range(0, sticks.Length)].Drop();
        sticks[Random.Range(0, sticks.Length)].Drop();
    }
}
