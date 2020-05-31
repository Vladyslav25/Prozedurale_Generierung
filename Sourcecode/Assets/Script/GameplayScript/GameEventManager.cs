using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    /// <summary>
    /// the current GameEventManager
    /// </summary>
    public static GameEventManager current;

    private void Awake()
    {
        current = this;
    }

    public event Action onPickUp; //When Player PickUp the Sword

    public bool isEquip;

    public void EquipOn()
    {
        onPickUp?.Invoke();
        isEquip = true;
    }

    public event Action onGetPoint; //When Player get a kill
    public void GetPoint()
    {
        onGetPoint?.Invoke();
    }

}
