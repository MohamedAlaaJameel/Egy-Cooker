using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public enum MeatStates
{
   raw,girlled,burned
}

public interface ICommand
{
    void Execute(Meat food);
}
public class SendToTableCommand : ICommand
{
    private MyTable _table;
    public SendToTableCommand(MyTable table)
    {
        _table = table;
    }
    public void Execute(Meat food)
    {
        food.ClearEvents();

        if (!_table.IsAvailable)
        {
            return;
        }
     
        var clock = food.GetComponentInChildren<Clock>();
        if (clock!=null)
        {
          clock.StopClock();
        }
        _table.AddToTable(food);
    }
}
public class SendToOvenCommand : ICommand
{
    Oven _oven;
    public SendToOvenCommand(Oven oven)
    {
        _oven = oven;
    }
    public void Execute(Meat food)
    {
        food.ClearEvents();
        _oven.AddFood(food);
    }
}
public class SendToTrashCommand : ICommand
{
    Trash _trash;
    public SendToTrashCommand(Trash trash)
    {
        _trash = trash;
    }
    public void Execute(Meat food)
    {
        food.ClearEvents();
        Debug.Log("send to trash has been called");
        _trash.AddFood(food);
    }
}



public interface IChangeFoodStates
{
    public void ChangeFoodState(IFoodState gameObject,Meat food);//todo Ifood
}


public interface IFoodState
{
    void Handle(Meat meat);

}

public class RawState : IFoodState
{
    public void Handle(Meat food)
    {
        Debug.Log("Meat is raw.");
        food.sendToOvenCommand.Execute(food);
       // food.AddBTNCommand(food.sendToOvenCommand);


        //  food.currentCommand.Execute(food);
     }
}
public class GrilledState : IFoodState
{
   
    public void Handle(Meat food)
    {
        Debug.Log("Meat is grilled.");
         
        food.AddBTNCommand(food.sendToTableCommand);

        food.SetShape(food.grilledMeat);
 
    }
}
public class BurnedState : IFoodState
{
    public void Handle(Meat food)
    {
        food.AddBTNCommand(food.sendToTrashCommand);
        food.SetShape(food.burnedMeat);

        Debug.Log("Meat is burned.");
    }
}
 