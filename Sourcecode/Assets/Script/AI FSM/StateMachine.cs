using System.Collections.Generic;
using UnityEngine;
using States;

public class StateMachine : MonoBehaviour
{
    #region -For Raycast-
    public int segments = 12;
    public float curveAmount = 60f;
    #endregion

    private List<AState> validStats = new List<AState>();

    private NPC npc;
    private AState lastState; //will be used maybe later with more complex AI

    private Dictionary<StateType, AState> StatesDic; //a Dictionary with all possible States

    [HideInInspector]
    public AState currState { get; private set; }

    void Awake()
    {
        currState = null;
        StatesDic = new Dictionary<StateType, AState>();
    }

    private void Start()
    {
        npc = GetComponent<NPC>();
        //Create Instances of the valid States if empty

        validStats.Add((MovingState)ScriptableObject.CreateInstance(typeof(MovingState)));
        validStats.Add((FleeState)ScriptableObject.CreateInstance(typeof(FleeState)));
        validStats.Add((AttackState)ScriptableObject.CreateInstance(typeof(AttackState)));

        //Sets all the needed Variables
        foreach (AState state in validStats)
        {
            state.SetFSM(this);
            state.SetExecutingNPC(npc);
            state.SetNavMesh(npc.navMeshAgent);
            StatesDic.Add(state.type, state);
        }
        EnterState(StateType.MOVING); //Starting State = Moving
    }

    private void Update()
    {
        if (currState != null)
        {
            currState.UpdateState(); //Update the currState
        }
    }

    #region -State Managment-

    public void EnterState(AState _nextstate) //If you want to enter a State in Order (exit the oldState, enter the newState, update the newState)
    {
        if (_nextstate == null)
        {
            return;
        }
        if (currState != null)
        {
            currState.ExitState();
        }
        lastState = currState;
        currState = _nextstate;
        currState.EnterState();
    }

    public void EnterState(StateType _stateType) //If you want to enter a State in Order (exit the oldState, enter the newState, update the newState)
    {
        if (StatesDic.ContainsKey(_stateType)) //Findes the State in the Dictionary
        {
            AState nextState = StatesDic[_stateType]; //Take the State from the Dictionary

            EnterState(nextState);
        }
    }

    #endregion
}
