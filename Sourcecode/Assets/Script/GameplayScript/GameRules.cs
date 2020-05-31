using UnityEngine;

public class GameRules : MonoBehaviour
{
    int kills = 0;
    int maxKills;

    void Start()
    {
        maxKills = GameObject.FindGameObjectWithTag("CityArea").GetComponent<CityCreater>().NPC_count;
        GameEventManager.current.onGetPoint += GetKill; //Add Func to Event
    }

    private void GetKill()
    {
        kills++;
        if (kills >= maxKills) //Check if Player have killed all Zombies
        {
            MySceneManager.ChangeScene("Win");
        }
    }
}
