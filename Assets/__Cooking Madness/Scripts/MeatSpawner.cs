using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class MeatSpawner :MonoBehaviour
{
    [Inject]
    Meat.Factory meatfactory;
    [Inject]
    Oven oven;
    [Inject]
    SignalBus _signalBus;
    public void SpawnMeat()//binded to button
    {
        if (oven.IsAvailable)
        {
            var meat = meatfactory.Create();
            var onRawMeatSignal = new OnRawMeatCreationSignal { meatGameObject = meat };
            _signalBus.Fire(onRawMeatSignal);
        }
    }
}