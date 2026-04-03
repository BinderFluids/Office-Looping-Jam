using System.Collections.Generic;
using System.Linq;
using Registry;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.UnifiedRayTracing;
using UnityUtils;

namespace MicrogameSystem
{
    public interface IMicrogameContext
    {
        void StartMicrogame();
        void EndMicrogame();
    }
    
    public abstract class MicrogameContext<T> : MonoBehaviour, IMicrogameContext where T : MicrogameContext<T>
    {
        public EventWrapper.EventWrapper OnMicrogameStart = new();
        public EventWrapper.EventWrapper<bool> OnMicrogameEnd = new();
        public EventWrapper.EventWrapper OnMicrogameUpdate = new();
        public EventWrapper.EventWrapper OnMicrogameSucceed = new();
        public EventWrapper.EventWrapper OnMicrogameFail = new();
            
        public IEnumerable<MicrogameBehaviour<T>> Behaviours => Registry<MicrogameBehaviour<T>>.All;
        public static MicrogameContext<T> Instance { get; private set; }

        [SerializeField] private bool shouldStartWithWinConditionMet = false; 
        [SerializeField] private bool winConditionMet; 
        
        private void Awake()
        {
            Instance = this; 
            gameObject.SetActive(false);
        }

        public void Succeed()
        {
            if (winConditionMet) return;
            winConditionMet = true;
            
            OnMicrogameSucceed.Invoke();
            OnSucceed();
        }
        protected virtual void OnSucceed() {}
        
        public void Fail()
        {
            if (winConditionMet) return;
            winConditionMet = false; 
            
            
            OnMicrogameFail.Invoke();
            OnFail();
        }
        protected virtual void OnFail() {}
        
        public void StartMicrogame()
        {
            winConditionMet = shouldStartWithWinConditionMet; 
            gameObject.SetActive(true); 
            
            OnMicrogameStart.Invoke();
            OnStartMicrogame();
            Behaviours.ForEach(b => b.OnMicrogameStart());
        }
        protected virtual void OnStartMicrogame() { }

        private void Update()
        {
            if (!gameObject.activeSelf) return; 

            OnMicrogameUpdate.Invoke();
            for (int i = Behaviours.Count(); i > 0; i--)
            {
                MicrogameBehaviour<T> behaviour = Behaviours.ToList()[i]; 
                behaviour.OnMicrogameUpdate(Time.deltaTime);
            }
            OnUpdate();
        }

        protected virtual void OnUpdate() { }

        public void EndMicrogame()
        {
            gameObject.SetActive(false);
            
            Behaviours.ForEach(b => b.OnMicrogameEnd());
            OnMicrogameEnd.Raise(winConditionMet);
            OnEndMicrogame();
        }

        protected virtual void OnEndMicrogame() { }
    }
}
