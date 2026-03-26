using System;
using UnityEngine;
using Registry;
using UnityUtils;

namespace Draggable
{
    public class DraggableCursor : MonoBehaviour
    {
        [SerializeField] private InputReader input; 
        [SerializeField] private float cursorSize = 10f;

        private Vector2 offset; 
        private DraggableBehaviour target;
        
        private void Update()
        {
            if (input.Click.WasPressedThisFrame)
            {
                target =
                    Registry<DraggableBehaviour>
                        .Get(new Closest(cursorSize, input.MouseWorldPosition));

                if (target != null)
                {
                    offset = target.transform.position - input.MouseWorldPosition.ToVector3();
                }
            }

            if (target != null)
                target.transform.position = input.MouseWorldPosition + offset; 

            
            if (!input.Click.IsPressed)
                target = null;
        }
    }
}