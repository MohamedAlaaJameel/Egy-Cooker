using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Table : MonoBehaviour
{


    public bool IsAvailable => GetSlot() != null;
    public Transform GetSlot()
    {
        var slot = transform.Cast<Transform>().Where(placeholder => placeholder.childCount == 0).FirstOrDefault();
        return slot;
    }
    public void AddToTable(Meat meat)
    {
        var slot = GetSlot();
        if (slot != null)
        {
            meat.transform.SetParent(slot.transform);
        }
    }




}
