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
        
        [SerializeField] private bool canDrag = false;
        public void SetCanDrag(bool value)
        {
            canDrag = value;
            if (!canDrag)
                ReleaseTarget();
        }

        private void Update()
        {
            if (!canDrag) return;
            
            if (input.Click.WasPressedThisFrame)
            {
                target =
                    Registry<DraggableBehaviour>
                        .Get(new Closest(cursorSize, input.MouseWorldPosition));

                if (target != null)
                {
                    target.DragStart(); 
                    //offset = input.MouseWorldPosition.ToVector3() - target.transform.position;
                }
            }

            if (target != null)
            {
                target.Drag();
                target.transform.position = input.MouseWorldPosition;// + offset;
            }


            if (!input.Click.IsPressed)
                ReleaseTarget();
        }

        void ReleaseTarget()
        {
            
            target?.DragEnd();
            target = null;
        }
    }
}