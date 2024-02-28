using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public interface IInterActableFood
{
    void ClearEvents();
    void SetEvents();
}
public interface _2DFood:IInterActableFood
{
    void SetShape(Sprite shape);
}

public class Meat : MonoBehaviour, _2DFood
{
    public Sprite grilledMeat;
    public Sprite burnedMeat;
    private Image image;
    public Button button { get; set; }
    [field:SerializeField]
    public int GrillDuration { get; set; }
    [field: SerializeField]
    public int BurnDuration { get; set; }
 //private IFoodState _state;
    private Clock _foodclock;
   public  ICommand sendToOvenCommand { get; set; }
   public  ICommand sendToTableCommand { get; set; }
   public  ICommand sendToTrashCommand { get; set; }

    public void Start() //  i will call start maybe better .. 
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        var st = new RawState();
        TransitionTo(st);
    }
    public void SetShape(Sprite shape)
    {
        image.sprite = shape;
    }
    public void TransitionTo(IFoodState state)
    {
        ClearEvents();
        state.Handle(this);
        Debug.Log($"Transitioning to {state.GetType().Name}");

    }
    public void ClearEvents()
    {
        button.RemoveAllListenersSafe();
    }
    public void AddBTNCommand(ICommand command)
    {
        button.onClick.AddListener(() => command.Execute(this));
    }

    public void SetEvents()
    {
      //  throw new NotImplementedException();
    }

    public class Factory : PlaceholderFactory<Meat> { }
}
