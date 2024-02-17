using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    // Start is called before the first frame update
    int allOrders;
    int recvOrders;
    public List<Order> orders;
    public void LeaveHappy()
    {
        Debug.Log("Customer leaving & Happy");
        LevelManager.Instance.OnCustoemrLeave(this);
        //level manager .register leaver .. platescount-=1;
    }    
    public void SetOrders(List<Order> orders)
    {
        allOrders = orders.Count;
        foreach (var order in orders)
        {
            order.OrderObject.transform.SetParent(gameObject.transform);
            order.OrderObject.name = order.type.ToString();

        }
        this.orders = orders;
    }
    public void GiveFoodToCustomer(Order order)
    {
        Transform requiredFood = gameObject.transform.Find(order.type.ToString());
        if (requiredFood!=null)
        {
            var objectToremoved= gameObject.transform.Cast<Transform>().Where(t => t.GetComponent<Image>().sprite == order.OrderObject.GetComponent<Image>().sprite).First();
            DestroyImmediate(objectToremoved.gameObject);
            orders.Remove(order);
            recvOrders += 1;
            if (recvOrders == allOrders)
            {
                LeaveHappy();
            }
        }
        else
        {
            Debug.Log("Cant find customer food this is an error ");
        }
     
    }

}
