using System;
using UnityEngine;
using Registry;

namespace InteractionSystem
{
    public class InteractableBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool canInteract = true;
        public void SetInteract(bool value) => canInteract = value;

        [SerializeField] private EventWrapper.EventWrapper OnInteractWrapper = new();

        private void Awake()
        {
            Registry<IInteractable>.TryAdd(this); 
        }

        public void Interact()
        {
            if (!gameObject.activeSelf) return;
            if (!canInteract) return;
            
            OnInteractWrapper.Invoke(); 
            OnInteract();
        }
        protected virtual void OnInteract() { }

        private void OnDestroy()
        {
            Registry<IInteractable>.Remove(this);
        }
    }
}