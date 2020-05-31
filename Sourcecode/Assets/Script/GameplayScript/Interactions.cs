using UnityEngine;

public class Interactions : MonoBehaviour
{
    private GameObject Player;
    private GameObject Sword_Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Sword_Player = GameObject.FindGameObjectWithTag("Sword");

        Sword_Player.SetActive(false);

        GameEventManager.current.onPickUp += PickUpSword; //Add Func to Event
    }

    private void PickUpSword()
    {
        //this case should beallways true
        if (Player.tag == "Player")
        {
            //when the player pick up the sword
            Player.tag = "Evil";
            Sword_Player.SetActive(true);
        }
    }
}
