using EventBus;
using Events;
using UnityEngine;

namespace LevelGeneration.StructureActions
{
    public class TeleportPlayerOnCreation : MonoBehaviour
    {
        public void Awake()
        {
            EventBus<TeleportPlayerEvent>.Raise(new TeleportPlayerEvent()
            {
                Position = transform.position
            });
        }
    }
}