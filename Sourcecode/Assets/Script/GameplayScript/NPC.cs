using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(StateMachine))]
public class NPC : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector]
    public StateMachine stateMachine; 

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = GetComponent<StateMachine>();
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
