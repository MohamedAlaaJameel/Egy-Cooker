using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ClockSpawner : IInitializable,IDisposable
{

    [Inject]
    Clock.Factory clockFactory;

    [Inject]
    SignalBus _signalBus;

    public void SpawnClock(OnPlacedFoodOnOvenSignal args)//binded to button
    {
     
       var clock = clockFactory.Create();

        clock.transform.SetParent(args.meat.transform);
        clock.SetMeatStateChanger(args.meat);
        args.meat.SetClockControl(clock);
    }

    public void Initialize()
    {
        _signalBus.Subscribe<OnPlacedFoodOnOvenSignal>(SpawnClock);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<OnPlacedFoodOnOvenSignal>(SpawnClock);
    }
}
