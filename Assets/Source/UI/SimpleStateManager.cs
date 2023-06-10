using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleStateManager : MonoBehaviour
{
    [System.Serializable]
    public class SimpleState
    {
        public string StateName;
        public UnityEvent Events;
    }

    public List<SimpleState> States = new List<SimpleState>();

    [SerializeField] private string _lastState;

    public void SetState(string stateName)
    {
        if (_lastState == stateName) return;
        foreach (var state in States)
        {
            if (state.StateName == stateName)
            {
                state.Events.Invoke();
                return;
            }
        }

    }
}
