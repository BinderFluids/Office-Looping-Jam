using System;
using EventBus;
using Events;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    
    EventBinding<TeleportPlayerEvent> teleportPlayerBinding;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        teleportPlayerBinding = new EventBinding<TeleportPlayerEvent>(TeleportPlayer);
        EventBus<TeleportPlayerEvent>.Register(teleportPlayerBinding);
    }

    private void UnsubscribeFromEvents()
    {
        EventBus<TeleportPlayerEvent>.Deregister(teleportPlayerBinding);
    }

    private void TeleportPlayer(TeleportPlayerEvent e)
    {
        playerPrefab.transform.position = e.Position;
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
}
