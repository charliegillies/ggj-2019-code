using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] destinationList;
    [SerializeField]
    private NavMeshAgent[] agentList;
    [SerializeField]
    private Campfire campfire;

    private bool isGameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {

        agentList = GetComponentsInChildren<NavMeshAgent>();
        Random.InitState((int)(System.DateTime.Now.Ticks));

        for (int i = 0; i < agentList.Length; i++)
        {
            agentList[i].SetDestination(destinationList[Random.Range(0, 4)].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < agentList.Length; i++)
        {
            if((agentList[i].transform.position - agentList[i].destination).sqrMagnitude < 1)
            {
               agentList[i].SetDestination(destinationList[Random.Range(0,4)].position);
            }
        }
    }

    public void startEndGame()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            campfire.OnGameOver();
        }
    }
}
