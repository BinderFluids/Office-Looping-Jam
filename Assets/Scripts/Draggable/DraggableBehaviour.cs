using System;
using UnityEngine;
using Registry;

public class DraggableBehaviour : MonoBehaviour
{
    private void Awake()
    {
        Registry<DraggableBehaviour>.TryAdd(this); 
    }
    
    
    private void OnDestroy()
    {
        Registry<DraggableBehaviour>.Remove(this); 
    }
}