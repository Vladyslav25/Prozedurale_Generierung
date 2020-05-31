using System.Collections.Generic;
using UnityEngine;
using States;

[CreateAssetMenu(fileName = "AttackState", menuName = "States/Attack", order = 3)]
public class AttackState : AState
{
    private GameObject Player;
    private List<RaycastHit> hits;

    public override void OnEnable()
    {
        hits = new List<RaycastHit>();

        base.OnEnable();
        type = StateType.ATTACK;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public override bool EnterState()
    {
        EnteredState = false;
        if (base.EnterState())
        {
            if (Player == null)
            {
                Debug.LogError("Es wurde kein Spieler in der AttackState gefunden");
            }
            EnteredState = true;
            navAgent.speed = 4f; //NPC speed up
            return EnteredState;
        }
        return base.EnterState(); //if the baseEnter fail
    }

    public override void UpdateState()
    {
        if (EnteredState)
        {
            navAgent.SetDestination(Player.transform.position);
            CastRay();
        }
    }

    public override bool ExitState()
    {
        navAgent.speed = 2f; //NPC slow down
        return base.ExitState();
    }

    public void CastRay() //Cast a Ray to the Player, if the Player get "invisible" or a obstacle is between it change the State to Move
    {
        RaycastHit hit;
        if (Physics.Raycast(npc.transform.position, Player.transform.position - npc.transform.position, out hit))
        {
            Debug.DrawLine(npc.transform.position, Player.transform.position);
            if (hit.transform.tag != "Player")
            {
                FSM.EnterState(StateType.MOVING); //Change to Moving if no Player found
            }
        }
    }
}
