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
    MyTable table;
    [Inject]
    Trash trash;

    public void SpawnMeat()//binded to button
    {
        if (oven.IsAvailable)
        {
            var meat= meatfactory.Create();
            ICommand sendToOvenCommand = new SendToOvenCommand(oven);
            ICommand sendToTableCommand = new SendToTableCommand(table);
            ICommand sendToTrashCommand = new SendToTrashCommand(trash);
            meat.sendToOvenCommand=sendToOvenCommand;
            meat.sendToTableCommand = sendToTableCommand;
            meat.sendToTrashCommand = sendToTrashCommand;

        }
    }

}