using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    // Dictionary to store events
    private Dictionary<GameEvents, Action<object>> eventDictionaryWithParams = new Dictionary<GameEvents, Action<object>>();
    // With parameters
    private Dictionary<GameEvents, Action> eventDictionaryWithoutParams = new Dictionary<GameEvents, Action>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        ClearAllEvents();
    }
#region  Event with parameters
    public static void ConnectEvent(GameEvents eventName, Action<object> listener)
    {
        if (Instance.eventDictionaryWithParams.TryGetValue(eventName, out Action<object> thisEvent))
        {
            thisEvent += listener;
            Instance.eventDictionaryWithParams[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            Instance.eventDictionaryWithParams.Add(eventName, thisEvent);
        }
    }

    public static void DisconnectEvent(GameEvents eventName, Action<object> listener)
    {
        if (Instance == null) return;

        if (Instance.eventDictionaryWithParams.TryGetValue(eventName, out Action<object> thisEvent))
        {
            thisEvent -= listener;
            if (thisEvent == null)
            {
                Instance.eventDictionaryWithParams.Remove(eventName);
            }
            else
            {
                Instance.eventDictionaryWithParams[eventName] = thisEvent;
            }
        }
    }
#endregion

#region Event without parameters
    public static void ConnectEvent(GameEvents eventName, Action listener)
    {
        if (Instance.eventDictionaryWithoutParams.TryGetValue(eventName, out Action thisEvent))
        {
            thisEvent += listener;
            Instance.eventDictionaryWithoutParams[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            Instance.eventDictionaryWithoutParams.Add(eventName, thisEvent);
        }
    }
    // Method to stop listening to an event without parameters
    public static void DisconnectEvent(GameEvents eventName, Action listener)
    {
        if (Instance == null) return;

        if (Instance.eventDictionaryWithoutParams.TryGetValue(eventName, out Action thisEvent))
        {
            thisEvent -= listener;
            if (thisEvent == null)
            {
                Instance.eventDictionaryWithoutParams.Remove(eventName);
            }
            else
            {
                Instance.eventDictionaryWithoutParams[eventName] = thisEvent;
            }
        }
    }
#endregion
    // Method to trigger an event with parameters
    public static void FireEvent(GameEvents eventName, object eventParam = null)
    {
        if (Instance.eventDictionaryWithParams.TryGetValue(eventName, out Action<object> thisEventWithParams))
        {   
            thisEventWithParams.Invoke(eventParam);
        }

        if (Instance.eventDictionaryWithoutParams.TryGetValue(eventName, out Action thisEventWithoutParams))
        {
            thisEventWithoutParams.Invoke();
        }
    }

    /// <summary>
    /// Do not call this unless you know what you are doing, events should be disconnected on their own scripts OnDisable or OnDestroy
    /// </summary>
    public static void ClearAllEvents()
    {
        Instance.eventDictionaryWithParams.Clear();
        Instance.eventDictionaryWithoutParams.Clear();
    }
}