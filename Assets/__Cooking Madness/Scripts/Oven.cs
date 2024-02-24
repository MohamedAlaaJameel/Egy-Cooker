using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Oven : MonoBehaviour
{
    [Inject]
    readonly SignalBus _signalBus;
    private void OnEnable()
    {
        _signalBus.Subscribe<OnRawMeatCreationSignal>(AddToOven);

    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<OnRawMeatCreationSignal>(AddToOven);
    }
    public bool IsAvailable => GetSlot()!=null;
    public Transform GetSlot()
    {
        var slot = transform.Cast<Transform>().Where(placeholder => placeholder.childCount == 0).FirstOrDefault();
        return slot;
    }
    public void AddToOven(OnRawMeatCreationSignal signal)
    {
        var slot = GetSlot();
        if (slot != null)
        {
            signal.meatGameObject.transform.SetParent(slot.transform);
            var foodOnOven = new OnPlacedFoodOnOvenSignal { meat = signal.meatGameObject };
            _signalBus.Fire(foodOnOven);
        }

    }




}
