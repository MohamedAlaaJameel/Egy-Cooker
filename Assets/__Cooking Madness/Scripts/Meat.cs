using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Meat : MonoBehaviour, IMeatStateChanger
{
    public Sprite grilledMeat;
    public Sprite burnedMeat;

    [Inject]
    SignalBus _signals;
    [Inject]
    Table _table;

    Image image;
    public Button button { get; set; }
    private void Start()
    {
        image= GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private IClockControl _clockControl;

    public void SetClockControl(IClockControl clockControl)
    {
        _clockControl = clockControl;
    }
    public void ChangeToGrilled()
    {
        image.sprite = grilledMeat;
        var meatBTN = GetComponent<Button>();
        meatBTN.RemoveAllListenersSafe();
        meatBTN.onClick.AddListener(() => SendToTableFunc());

    }

    public void ChangeToBurned()
    {
        var meatBTN = GetComponent<Button>();
        meatBTN.RemoveAllListenersSafe();

        GetComponent<Image>().sprite = burnedMeat;
         meatBTN.onClick.AddListener(() => DropMeat());    
    }

    private void DropMeat()
    {
        DestroyImmediate(this.gameObject);
    }

    public void SendToTableFunc()//binded to button
    {
        if (_table.IsAvailable)
        {
            _clockControl?.StopClock();
            button.RemoveAllListenersSafe();
            //button.onClick.AddListener(() => SendToCustomerFunc()); 

            _table.AddToTable(this);
        }
    }

    public class Factory : PlaceholderFactory<Meat> { }
}
