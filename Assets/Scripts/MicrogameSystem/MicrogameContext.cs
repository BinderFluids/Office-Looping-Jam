using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public IEnumerable<MicrogameBehaviour<T>> Behaviours => behaviours;
        [SerializeField] private List<MicrogameBehaviour<T>> behaviours = new();
        public EventWrapper.EventWrapper<MicrogameBehaviour<T>> OnAddBehaviour = new();
        public EventWrapper.EventWrapper<MicrogameBehaviour<T>> OnRemoveBehaviour = new();
        
        private List<MicrogameBehaviour<T>> queuedBehaviours = new(); 
        
        public void AddBehaviour(MicrogameBehaviour<T> behaviour)
        {
            if (behaviours.Contains(behaviour) || queuedBehaviours.Contains(behaviour)) return;
            queuedBehaviours.Add(behaviour);
        }

        public void RemoveBehaviour(MicrogameBehaviour<T> behaviour)
        {
            if (!behaviours.Contains(behaviour)) return; 
            behaviours.Remove(behaviour);
            OnRemoveBehaviour.Raise(behaviour);
        }
        
        
        void FillBehavioursFromQueued()
        {
            foreach (MicrogameBehaviour<T> behaviour in queuedBehaviours)
            {
                if (behaviours.Contains(behaviour)) continue;
                behaviours.Add(behaviour);
                OnAddBehaviour.Raise(behaviour);
            }
            queuedBehaviours.Clear();
        }
        
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
                MicrogameBehaviour<T> behaviour = behaviours[i]; 
                behaviour.OnMicrogameUpdate(Time.deltaTime);
            }
            FillBehavioursFromQueued();
            
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
