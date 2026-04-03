using System;
using Registry;
using UnityEngine;

namespace MicrogameSystem
{
    public abstract class MicrogameBehaviour<T> : MonoBehaviour where T : MicrogameContext<T>
    {
        protected MicrogameContext<T> ctx => MicrogameContext<T>.Instance; 

        protected virtual void Awake()
        {
            Registry<MicrogameBehaviour<T>>.TryAdd(this); 
        }
    
        public virtual void OnMicrogameStart() { }
        public virtual void OnMicrogameUpdate(float dt) { }
        public virtual void OnMicrogameEnd() { }
        
        protected virtual void OnDestroy()
        {
            Registry<MicrogameBehaviour<T>>.Remove(this); 
        }
    }
}