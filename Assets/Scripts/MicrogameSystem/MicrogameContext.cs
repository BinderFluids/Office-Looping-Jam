using System.Collections.Generic;
using Registry;
using UnityEngine;
using UnityUtils;

namespace MicrogameSystem
{
    public abstract class MicrogameContext<T> : MonoBehaviour where T : MicrogameContext<T>
    {
        public EventWrapper.EventWrapper OnMicrogameStart = new();
        public EventWrapper.EventWrapper OnMicrogameEnd = new();
        private IEnumerable<MicrogameBehaviour<T>> behaviours; 
    
        private void Awake()
        { 
            gameObject.SetActive(false);
        }

        private void Start()
        {
            behaviours = Registry<MicrogameBehaviour<T>>.All;
        }

        public void StartMicrogame()
        {
            gameObject.SetActive(true); 
            OnMicrogameStart.Invoke();
            OnStart();
        }
        protected abstract void OnStart();

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                behaviours.ForEach(b => b.OnMicrogameUpdate(Time.deltaTime));
                OnUpdate();
            }
        }
        protected abstract void OnUpdate();

        public void EndMicrogame()
        {
            gameObject.SetActive(false);
            OnMicrogameEnd.Invoke();
            OnEnd();
        }
        protected abstract void OnEnd();
    }
}
