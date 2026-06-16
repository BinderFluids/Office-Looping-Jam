using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;


public class ScriptableEvent<T> : ScriptableObject
{
    readonly List<IGameEventListener<T>> listeners = new();
    
    public void Raise(T data)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(data);
    }
    
    public void RegisterListener(IGameEventListener<T> listener) => listeners.Add(listener);
    public void UnregisterListener(IGameEventListener<T> listener) => listeners.Remove(listener);
}

[CreateAssetMenu(menuName = "Scriptable Events/Empty Event")]
public class ScriptableEvent : ScriptableEvent<Unit>
{
    public void Raise() => Raise(Unit.Default);
}

public struct Unit
{
    public static Unit Default => default;
}