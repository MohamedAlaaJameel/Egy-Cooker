using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrderType 
{
    grilledBeef,Juice

}
public class Order
{
    public GameObject OrderObject;
    public OrderType type;

    public Order(GameObject orderObject, OrderType type)
    {
        OrderObject = orderObject;
        this.type = type;
    }
}
