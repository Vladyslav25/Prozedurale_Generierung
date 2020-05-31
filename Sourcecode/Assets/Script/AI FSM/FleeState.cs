using UnityEngine;
using States;
using System;

[CreateAssetMenu(fileName = "FleeState", menuName = "States/Flee", order = 4)]
public class FleeState : AState
{
    private Vector3 currDestination;
    private Bounds cityBounds;
    private Vector3 lookingDir;
    private bool reachedPos;
    private GameObject Player;

    public GameObject CityArea;

    public override void OnEnable()
    {
        base.OnEnable();
        cityBounds = GameObject.FindGameObjectWithTag("CityArea").GetComponent<CityCreater>().cityBounds;
        type = StateType.FLEE;
    }

    public override bool EnterState()
    {
        EnteredState = false;
        if (base.EnterState())
        {
            if (navAgent == null)
            {
                throw new Exception("Es wurde kein NavMeshAngent in den State übergeben");
            }
            if (GameObject.FindGameObjectWithTag("Evil") == null)
            {
                throw new Exception("Es wurde kein Spieler mit dem Tag 'Evil' gefunden");
            }
            else
            {
                Player = GameObject.FindGameObjectWithTag("Evil");
            }
            EnteredState = true;
            navAgent.speed = 10f;
            lookingDir = npc.transform.forward;
            NewCurrDir();
            Wander();
            return EnteredState;
        }
        return base.EnterState(); //if the baseEnter fail
    }

    public override void UpdateState()
    {
        if (EnteredState)
        {
            Execute();
        }
        CheckForDistance();
    }

    private void CheckForDistance()
    {
        if (Vector3.Distance(Player.transform.position, npc.transform.position) > 15f) //if the Player is too far away it change to Moving
        {
            FSM.EnterState(StateType.MOVING);
        }
    }

    private bool CheckDestinationReached() //if it hit the Destination return true
    {
        float distanceToTarget = Vector3.Distance(npc.transform.position, currDestination);
        if (distanceToTarget < 3f)
        {
            return true;
        }
        return false;
    }

    private void Execute()
    {
        if (Wander())
        {
            reachedPos = CheckDestinationReached();
        }
        if (reachedPos)
        {
            NewCurrDir();
        }
    }

    private void NewCurrDir() //Create a Random Pos in front of the Player, somewhere inside a circel
    {
        currDestination = (npc.transform.position - Player.transform.position) * 4f +
            Helper.GetUnitOnCircle(UnityEngine.Random.Range(0, 360),
            UnityEngine.Random.Range(1, 4)) + npc.transform.position;

        reachedPos = false;
    }

    private bool Wander() //casual AI Wander
    {
        if(cityBounds== null)
        {
            cityBounds = GameObject.FindGameObjectWithTag("CityArea").GetComponent<CityCreater>().cityBounds;
        }

        if (cityBounds.center != Vector3.zero) //for somereason its need to be here
        {
            bool FindDestination = false;
            int counter = 0;
            while (!FindDestination)
            {
                counter++;
                if (counter > 100) //if it trys too much, its breaks
                {
                    return false;
                }
                if (Helper.CheckForValidPos(currDestination, new BoxCollider())
                    && Helper.InsideArea(currDestination, cityBounds))
                {
                    FindDestination = true;
                    navAgent.SetDestination(currDestination);
                    return true;
                }
                NewCurrDir();
            }
        }
        return false;
    }
}
