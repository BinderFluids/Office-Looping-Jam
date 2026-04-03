using System;
using UnityEngine;
using Registry;

public class DraggableBehaviour : MonoBehaviour
{
    public EventWrapper.EventWrapper OnDragStart = new();
    public EventWrapper.EventWrapper OnDrag = new();
    public EventWrapper.EventWrapper OnDragEnd = new();

    private bool isBeingDragged; 
    public bool IsBeingDragged => isBeingDragged;
    
    private void Awake()
    {
        Registry<DraggableBehaviour>.TryAdd(this); 
    }

    public void DragStart()
    {
        isBeingDragged = true; 
        OnDragStart.Invoke();
    }

    public void Drag()
    {
        OnDrag.Invoke();
    }

    public void DragEnd()
    {
        isBeingDragged = false; 
        OnDragEnd.Invoke();
    }
    
    private void OnDestroy()
    {
        Registry<DraggableBehaviour>.Remove(this); 
    }
}