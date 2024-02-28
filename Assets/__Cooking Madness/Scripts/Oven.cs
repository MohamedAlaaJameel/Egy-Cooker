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
    private Clock.Factory clockFactory;

    private Clock _clock;

    public bool IsAvailable => GetSlot()!=null;
    public Transform GetSlot()
    {
        var slot = transform.Cast<Transform>().Where(placeholder => placeholder.childCount == 0).FirstOrDefault();
        return slot;
    }
 
    public void AddFood(Meat food)
    {
        food.transform.SetParent(GetSlot());
        _clock = clockFactory.Create();
        _clock.SetFood(food);

    }
}
