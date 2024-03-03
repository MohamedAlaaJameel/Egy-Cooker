using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Customer : MonoBehaviour
{
    public bool IsAvailable => GetSlot() != null;
    public Transform GetSlot()
    {
        var slot = transform.Cast<Transform>().Where(placeholder => placeholder.childCount == 0).FirstOrDefault();
        return slot;
    }

    public void AddFood(Meat food)
    {
        food.transform.SetParent(GetSlot());

    }

    public class Pool : MemoryPool<List<Meat>, Customer>
    {
    }
}