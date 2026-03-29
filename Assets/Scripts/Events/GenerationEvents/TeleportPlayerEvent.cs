using EventBus;
using UnityEngine;

namespace Events
{
    public struct TeleportPlayerEvent : IEvent
    {
        public Vector3 Position;
    }
}