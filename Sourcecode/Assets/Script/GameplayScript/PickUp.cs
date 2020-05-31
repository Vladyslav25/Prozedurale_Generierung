using UnityEngine;

public class PickUp : MonoBehaviour
{
    private void OnTriggerStay(Collider collision)
    {
        //On Pressed Key an Event is Called that the Player Pick up the Sword
        if (Input.GetButtonUp("Equip"))
        {
            GameEventManager.current.EquipOn();
            gameObject.SetActive(false);
        }
    }
}
