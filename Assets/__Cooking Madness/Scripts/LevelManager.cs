using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [Inject]
    private LevelUIManager levelUIManager;

    #region Inspector Fired Events
    //   [System.Serializable]
    //  public class ScoreChangedEvent : UnityEvent<string> { }

    // public UnityEvent<string> ScoreChangedEvent; 
    #endregion


    //UI Objects Injection 
    [Header("UI Injected Objects")]
    public TMPro.TextMeshProUGUI scoreMesh;

    [Header("Timer values")]
    public float cookDuration;// Duration to fill the bar
    public float beForeFireDuration; // Duration to fill the bar

    [Header("Others")]

    public GridLayoutGroup customerPanel;
    public GameObject customerPrefab;

    //score
    [field: SerializeField]
    public int platesMaxScore { get; set; }
    [field: SerializeField]
    public int currentPlatesScore { get; set; }


    public int maxSpawnedCustomers;
    [field: SerializeField]
    public int currentSpawnedCustoerms { get; set; }
    List<Order> TableOrders = new List<Order>();
    List<Customer> SpawnedCustomers = new List<Customer>();


    public GameObject meat;
    public GameObject placeHolder;


    public GameObject oven;
    public GameObject table;


    public int maxOvenSlots;
    public int currvOvenSlots;


    public int maxTableSlots;
    public int currTableSlots;

    // Singleton instance
    private static LevelManager instance;
    public Sprite rawMeat; // Assign this in the Inspector
    public Sprite grilledMeat; // Assign this in the Inspector
    public Sprite burnedMeat; // Assign this in the Inspector


    IEventManager eventManager;

    private void Awake()
    {
        Debug.Log("Level  awake has been called");
        InjectDependencies();
    }
 

    private void Start()
    {
        StartCoroutine(CustomerSpawner());
    }

    public void InjectDependencies()
    {
        eventManager = EventManager.Instance;
        LevelUIManager.Instance.SetEventManager(eventManager);
        ITextProvider textProvider = new TextMeshProProvider(scoreMesh);
      //  LevelUIManager.Instance.SetTextProvider(textProvider);


    }
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(LevelManager).Name;
                    instance = obj.AddComponent<LevelManager>();
                }
            }
            return instance;
        }
    }

    // Private constructor to prevent instantiation
    private LevelManager() { }
    public void PlaceRawMeatOnOven()
    {
        if (currvOvenSlots < maxOvenSlots)
        {
            var emptySlot = oven.transform.Cast<Transform>().Where(t => t.childCount == 0).ToList().FirstOrDefault();
            if (emptySlot)
            {
                var rawMeatInstance = Instantiate(meat, emptySlot.transform);
                Order order = new Order(rawMeatInstance, OrderType.grilledBeef);

                var Clock = rawMeatInstance.GetComponentInChildren<LoadingBarTimer>(true);
                Clock.SetOrder(order);

                Clock.gameObject.SetActive(true);
                var meatBTN = rawMeatInstance.GetComponent<Button>();
                RemoveButtonListeners(meatBTN);
                currvOvenSlots += 1;
            }
            else
            {
                Debug.Log("unkown reason error there is no empty slots ");
            }
           
        }
        else
        {
            Debug.Log("LevelManager: Oven is busy => max slots");
        }
    }
    public void MoveMeatToTable(Order order)
    {
        var currentMeat = order.OrderObject;
        if (currentMeat != null && currTableSlots < maxTableSlots)
        {
            var emptySlot = table.transform.Cast<Transform>().Where(t => t.childCount == 0).ToList().FirstOrDefault();
            if (emptySlot)
            {
                TableOrders.Add(order);



                currvOvenSlots -= 1;
                currTableSlots += 1;
                var Clock = currentMeat.GetComponentInChildren<LoadingBarTimer>(true);
                Clock.gameObject.SetActive(false);
                var meatBTN = currentMeat.GetComponent<Button>();
                RemoveButtonListeners(meatBTN);
                meatBTN.onClick.AddListener(() => { GiveMeatToCustomer(order); });
                currentMeat.transform.SetParent(emptySlot.transform);
            }
            else
            {
                Debug.Log("unkown reason error there is no empty slots or placeholders on Table");

            }
        }
    }
    public void ChangeToGrilled(Order order)
    {
        order.OrderObject.GetComponent<Image>().sprite = grilledMeat;
        var meatBTN = order.OrderObject.GetComponent<Button>();
        meatBTN.onClick.AddListener(() => MoveMeatToTable(order));
    }
    public void ChangeToBurned(Order order)
    {
        var meatBTN = order.OrderObject.GetComponent<Button>();
        RemoveButtonListeners(meatBTN);
        order.OrderObject.GetComponent<Image>().sprite = burnedMeat;
        meatBTN.onClick.AddListener(() => DropMeat(order.OrderObject));
    }
    public void DropMeat(GameObject meat)
    {
        Debug.Log("Dropping to trash");
        currvOvenSlots -= 1;
        DestroyImmediate(meat);
    }
    public void GiveMeatToCustomer(Order order)
    {
        Debug.Log("customer got the meat");
        var requiredCustomer = SpawnedCustomers.FirstOrDefault(c => c.orders != null && c.orders.Any(ord => ord.type == order.type));

        if (requiredCustomer != null)
        {
            currentPlatesScore += 1;

            var score = $"{currentPlatesScore}/{platesMaxScore}";
        //    var scoreChangeEventArg = new ScoreChangnedEventArg { scoreString = score };
            levelUIManager.SetScoreText(score);
            //// scoreText.text =;
            requiredCustomer.GiveFoodToCustomer(order);
            if (TableOrders.Contains(order))
            {
                TableOrders.Remove(order);
            }
            DestroyImmediate(order.OrderObject);
            currTableSlots -= 1;

        }
    }

    private static void RemoveButtonListeners(Button meatBTN)
    {
        if (meatBTN!=null)
        {
            meatBTN.onClick.RemoveAllListeners();
            int persistentCount = meatBTN.onClick.GetPersistentEventCount();
            for (int i = 0; i < persistentCount; i++)
            {
                // Since removing a listener will shift the indices, always remove the first one
                UnityEventTools.RemovePersistentListener(meatBTN.onClick, 0);
            }
        }
        else
        {
            Debug.LogError("Button is Null");
        }
    }




    public Customer SpawnACustomerWithOrders()
    {
        //todo level requirement list like meat juce tomato ... and randomize the orders ... 
        //todo make some customers take the table orders
        var customerObject = Instantiate(customerPrefab, customerPanel.transform);
        var orderType = OrderType.grilledBeef.ToString();
        var orderImageObject= MakeOrderImage(orderType, customerObject.transform);
        var orderImageObject2= MakeOrderImage(orderType, customerObject.transform);
 

        Customer customer = customerObject.GetComponent<Customer>();

        Order order = new Order(orderImageObject, OrderType.grilledBeef);
        Order order2 = new Order(orderImageObject2, OrderType.grilledBeef);
        List<Order> orders = new List<Order>
        {
            order,
            order2
        };
        customer.SetOrders(orders);
        return customer;
    }

    private GameObject MakeOrderImage(string orderType,Transform customerTranform)
    {
        string OrderName = orderType;
        GameObject imageObj = new GameObject(OrderName);
        imageObj.transform.SetParent(customerTranform);
        Image imageComponent = imageObj.AddComponent<Image>();
        imageComponent.sprite = grilledMeat;
        return imageObj;
    }

    IEnumerator CustomerSpawner()
    {
        while (currentPlatesScore < platesMaxScore)
        {
            if (currentSpawnedCustoerms < maxSpawnedCustomers)
            {
                SpawnedCustomers.Add(SpawnACustomerWithOrders());
                currentSpawnedCustoerms++;
            }
            // Wait for 3 seconds before the next iteration
            yield return new WaitForSeconds(6f);
        }
    }
    public void OnCustoemrLeave(Customer customer)
    {
        SpawnedCustomers.Remove(customer);
        DestroyImmediate(customer.gameObject);
        currentSpawnedCustoerms--;
    }
    

}

