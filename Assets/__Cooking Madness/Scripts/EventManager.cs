using System;
using System.Collections.Generic;
using UnityEngine;


public interface IEventManager
{
    void Subscribe<T>(Action<T> listener);
    void Unsubscribe<T>(Action<T> listener);
    void InvokeEvent<T>(T eventParam);
}
 

public class EventManager : MonoBehaviour, IEventManager
{
    private static EventManager _instance;
    private Dictionary<Type, Delegate> _eventDictionary = new Dictionary<Type, Delegate>();

    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // This will only happen the first time this reference is used.
                var manager = FindObjectOfType<EventManager>();
                if (manager == null)
                {
                    GameObject go = new GameObject("EventManager");
                    _instance = go.AddComponent<EventManager>();
                }
                else
                {
                    _instance = manager;
                }

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Subscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);
        if (_eventDictionary.TryGetValue(eventType, out var del))
        {
            _eventDictionary[eventType] = Delegate.Combine(del, listener);
        }
        else
        {
            _eventDictionary[eventType] = listener;
        }
    }

    public void Unsubscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);
        if (_eventDictionary.TryGetValue(eventType, out var del))
        {
            Delegate currentDel = Delegate.Remove(del, listener);
            if (currentDel == null)
            {
                _eventDictionary.Remove(eventType);
            }
            else
            {
                _eventDictionary[eventType] = currentDel;
            }
        }
    }

    public void InvokeEvent<T>(T eventParam)
    {
        Type eventType = typeof(T);
        if (_eventDictionary.TryGetValue(eventType, out var del))
        {
            if (del is Action<T> callback)
            {
                callback(eventParam);
            }
            else
            {
                Debug.LogError($"InvokeEvent error: Event '{eventType.Name}' has no listener of type Action<{eventType.Name}>.");
            }
        }
        else
        {
            Debug.LogError($"InvokeEvent error: No subscribers for event '{eventType.Name}'.");
        }
    }
}
