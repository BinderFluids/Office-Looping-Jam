using System;
using UnityEngine;

public interface IGameEventListener<T>
{
    void OnEventRaised(T data);
}

public class GameEventListener<T> : MonoBehaviour, IGameEventListener<T>
{
    [SerializeField] private ScriptableEvent<T> scriptableEvent;
    public EventWrapper.EventWrapper<T> response;

    private void OnEnable() => scriptableEvent.RegisterListener(this);
    private void OnDisable() => scriptableEvent.UnregisterListener(this);

    public void OnEventRaised(T data) =>
        response.Raise(data); 
}