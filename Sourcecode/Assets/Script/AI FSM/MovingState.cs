using UnityEngine;
using States;
using System;

[CreateAssetMenu(fileName = "MovingState", menuName = "States/Moving", order = 1)]
public class MovingState : AState
{
    private Vector3 currDestination;
    private Bounds cityBounds;
    private Vector3 lookingDir;
    private bool reachedPos;

    public GameObject CityArea;

    public override void OnEnable()
    {
        base.OnEnable();
        cityBounds = GameObject.FindGameObjectWithTag("CityArea").GetComponent<CityCreater>().cityBounds;
        type = StateType.MOVING;
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
            EnteredState = true;
            navAgent.speed = 2f;
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
            CastRays();
        }
    }

    private void Execute()
    {
        // if a valid Pos was found
        if (Wander())
        {
            reachedPos = CheckDestinationReached();
        }
        //when reached, look for a new Pos to move
        if (reachedPos)
        {
            NewCurrDir();
        }
    }

    /// <summary>
    /// Get a Random Position for the AI to Move
    /// </summary>
    private void NewCurrDir()
    {
        currDestination = npc.transform.position + npc.transform.forward * 3 +
            Helper.GetUnitOnCircle(UnityEngine.Random.Range(0, 360),
            UnityEngine.Random.Range(1, 4));
    }

    private bool Wander()
    {
        if(cityBounds == null)
        {
            cityBounds = GameObject.FindGameObjectWithTag("CityArea").GetComponent<CityCreater>().cityBounds;
        }

        if (cityBounds.center != Vector3.zero)
        {
            bool FindDestination = false;
            int counter = 0;
            while (!FindDestination)
            {
                //Counter to break the while Loop
                counter++;
                if (counter > 100)
                {
                    return false;
                }
                //Check if the new Pos is valid
                if (Helper.CheckForValidPos(currDestination, new BoxCollider())
                    && Helper.InsideArea(currDestination, cityBounds))
                {
                    FindDestination = true;
                    navAgent.SetDestination(currDestination);
                    //return true if a new valid Pos was found
                    return true;
                }
                //If Pos is not valid, get an new one
                NewCurrDir();
            }
        }
        return false;
    }

    /// <summary>
    /// Check if the Destination is reached
    /// </summary>
    /// <returns>true if reached</returns>
    private bool CheckDestinationReached()
    {
        float distanceToTarget = Vector3.Distance(npc.transform.position, currDestination);
        if (distanceToTarget < 3f)
        {
            return true;
        }
        return false;
    }

    private void CastRays() //Looking far
    {
        calcAngle = 0f;
        for (int i = 0; i < segments; i++)
        {
            calcAngle = (-curveAmount / 2) + (curveAmount / segments) * i;
            Vector3 dir = Quaternion.Euler(0, calcAngle, 0) * npc.transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(npc.transform.position, dir, out hit, float.PositiveInfinity))
            {
                Debug.DrawLine(npc.transform.position, hit.point);
                if (hit.transform.tag == "Evil")
                {
                    //Change State to Flee if Player have sword
                    FSM.EnterState(StateType.FLEE);
                }
                if (hit.transform.tag == "Player")
                {
                    //Attack the Player if he have no sword
                    FSM.EnterState(StateType.ATTACK);
                }
            }
        }
    }
}

