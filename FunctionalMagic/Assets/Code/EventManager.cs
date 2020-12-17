using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, List<Action>> _events = new Dictionary<string, List<Action>>();

    public void DispatchEvent(string eventKey)
    {
        if(_events.TryGetValue(eventKey, out List<Action> actionList))
        {
            foreach(Action listener in actionList)
            {
                listener();
            }
        }
    }

    public void AddEventListener(string eventKey, Action listener)
    {

        List<Action> actionList;
        if(!_events.TryGetValue(eventKey, out actionList))
        {
            actionList = new List<Action>();
            _events.Add(eventKey, actionList);
        }
        actionList.Add(listener);
    }

    public void RemoveEventListener(string eventKey, Action listener)
    {
        List<Action> actionList;
        if(!_events.TryGetValue(eventKey, out actionList))
        {
            return;
        }
        actionList.Remove(listener);
    }
}
