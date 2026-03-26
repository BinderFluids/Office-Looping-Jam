using System;
using System.Collections.Generic;
using UnityEngine;
using Registry;

namespace InteractionSystem
{
    public class ProximityInteractionAgent : MonoBehaviour
    {
        [SerializeField] private Transform _transform; 
        [SerializeField] private float interactionRange = 2f;
        [SerializeField] private InputReader input;
        [SerializeField] private GameObject interactionIndicator; 
        private IInteractable closestTarget; 
        
        private void Start()
        {
            input.Interact.Action += TryInteract;
            
            interactionIndicator.SetActive(false);
            interactionIndicator.transform.SetParent(null); 
        }
        
        private void TryInteract(bool wasPressed)
        {
            print(wasPressed);
            if (wasPressed) closestTarget?.Interact();
        }
        
        private void Update()
        {
            closestTarget = Registry<IInteractable>.Get(new Closest(interactionRange, _transform.position));
            if (closestTarget == null)
            {
                interactionIndicator.SetActive(false);
                return; 
            }
            
            if (closestTarget is Component component)
            {
                interactionIndicator.SetActive(true);
                interactionIndicator.transform.position = component.transform.position;
            }
            else
                interactionIndicator.SetActive(false);
        }

        
        private void OnDestroy()
        {
            input.Interact.Action -= TryInteract; 
        }
    }
}