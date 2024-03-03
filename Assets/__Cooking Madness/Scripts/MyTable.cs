using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MyTable : MonoBehaviour
{


    public bool IsAvailable => GetEmptySlot() != null;
    public Transform GetEmptySlot()
    {
        var slot = transform.Cast<Transform>().Where(placeholder => placeholder.childCount == 0).FirstOrDefault();
        return slot;
    }
 
    public void AddToTable(Meat meat)
    {
        var slot = GetEmptySlot();
        if (slot != null)
        {
            meat.transform.SetParent(slot.transform);
        }
    }

    public void AddCondiment(Condiment condiment)
    {
        List<Meat> tablePlates = transform.Cast<Meat>().Where(meatPlate => meatPlate != null&&meatPlate.canAddCondiment).ToList();
        if (tablePlates.Any())
        {
            var firstoccurnce = tablePlates.First();
            firstoccurnce.AddCondiment(condiment);
        }


    }


}
 