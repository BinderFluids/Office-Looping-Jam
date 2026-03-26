using Registry;
using UnityEngine;

namespace MicrogameSystem
{
    public abstract class MicrogameBehaviour<T> : MonoBehaviour where T : MicrogameContext<T>
    {
        private T ctx;

        private void Awake()
        {
            Registry<MicrogameBehaviour<T>>.TryAdd(this); 
        }

        public abstract void OnMicrogameUpdate(float dt);
    }
}