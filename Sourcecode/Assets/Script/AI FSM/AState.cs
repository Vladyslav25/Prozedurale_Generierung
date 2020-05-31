using UnityEngine;
using UnityEngine.AI;

namespace States
{
    public enum ExecutionState //will be used maybe later with more complex AI
    {
        NONE,
        ACTIV,
        COMPLETED,
        TERMINATED,
    };

    public enum StateType //To acsses the Dictionary
    {
        NONE,
        MOVING,
        FLEE,
        ATTACK,
    };

    public abstract class AState : ScriptableObject
    {

        public ExecutionState ExecutionState { get; protected set; }

        public StateType type { get; protected set; }

        public StateMachine FSM;

        public bool EnteredState; //if the Enter was successful

        protected NavMeshAgent navAgent;

        protected NPC npc;

        protected int segments;
        protected float curveAmount;

        protected float calcAngle;

        public bool successNPC = true;
        public bool successNav = true;
        public bool succsesFSM = true;

        public virtual void OnEnable()
        {
            ExecutionState = ExecutionState.NONE;
            type = StateType.NONE;
        }

        public virtual bool EnterState() //State Enter
        {
            segments = FSM.segments;       //For the Raycast
            curveAmount = FSM.curveAmount; //For the Raycast
            ExecutionState = ExecutionState.ACTIV;
            successNav = (navAgent != null);
            successNPC = (npc != null);
            succsesFSM = (FSM != null);
            return successNav && successNPC && succsesFSM; //checks if the all the needed Variables arent null
        }

        public abstract void UpdateState(); //State Update

        public virtual bool ExitState() //State Exit
        {
            ExecutionState = ExecutionState.COMPLETED;
            return true;
        }

        public void SetNavMesh(NavMeshAgent _navMeshAgent) //Set the NavMeshAgnet
        {
            if (_navMeshAgent != null)
            {
                navAgent = _navMeshAgent;
            }
        }

        public virtual void SetExecutingNPC(NPC _npc) //Set the NPC
        {
            if (_npc != null)
            {
                npc = _npc;
            }
        }

        public virtual void SetFSM(StateMachine _fsm) //Set the FSM
        {
            if (_fsm != null)
            {
                FSM = _fsm;
            }
        }
    }
}


