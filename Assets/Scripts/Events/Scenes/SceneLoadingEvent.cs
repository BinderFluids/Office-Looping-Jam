using EventBus;

namespace Events
{
    public struct SceneLoadingEvent : IEvent
    {
        public string sceneName;
    }
}