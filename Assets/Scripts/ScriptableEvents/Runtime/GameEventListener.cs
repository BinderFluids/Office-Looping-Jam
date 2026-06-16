using System;
using UnityEngine;
using UnityEngine.Events;

public interface IGameEventListener<T>
{
    void OnEventRaised(T data);
}

public class GameEventListener<T> : MonoBehaviour, IGameEventListener<T>
{
    [SerializeField] private ScriptableEvent<T> scriptableEvent;
    [Space]
    [SerializeField] private UnityEvent<T> response; 

    private void OnEnable() => scriptableEvent.RegisterListener(this);
    private void OnDisable() => scriptableEvent.UnregisterListener(this);

    public void OnEventRaised(T data) =>
        response?.Invoke(data); 
}