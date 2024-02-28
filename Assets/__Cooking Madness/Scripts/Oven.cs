using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public struct clockDoneSignal{}
public class Oven : MonoBehaviour
{
    [Inject]
    private Clock.Pool clockPool;

    private Clock _clock;

    Dictionary<int,Clock> clocks = new Dictionary<int, Clock>();
    public bool IsAvailable => GetSlot()!=null;
    public Transform GetSlot()
    {
        var slot = transform.Cast<Transform>().Where(placeholder => placeholder.childCount == 0).FirstOrDefault();
        return slot;
    }

    public void AddFood(Meat food)
    {
        food.transform.SetParent(GetSlot());
        _clock = clockPool.Spawn(food);
        clocks.Add(_clock.ID, _clock);

    }
    public void DespawnClock(int id)
    {
       
        clockPool.Despawn(clocks[id]);
        clocks.Remove(id);
    }

}
