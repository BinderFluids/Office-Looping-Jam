using EventBus;

namespace Events
{
    public struct SceneLoadedEvent : IEvent
    {
        public string sceneName;
    }
}