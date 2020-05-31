using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour //change the UI Text
{
    public Text ui_Find;
    public Text ui_Kill;
    public Text ui_Hunted;
    public Text ui_Hunter;

    private void Start()
    {
        ui_Find.gameObject.SetActive(true);
        ui_Hunted.gameObject.SetActive(true);

        ui_Kill.gameObject.SetActive(false);
        ui_Hunter.gameObject.SetActive(false);
        GameEventManager.current.onPickUp += ChangeToPickUp;
    }

    private void ChangeToPickUp()
    {
        ui_Find.gameObject.SetActive(false);
        ui_Hunted.gameObject.SetActive(false);

        ui_Kill.gameObject.SetActive(true);
        ui_Hunter.gameObject.SetActive(true);
    }
}
