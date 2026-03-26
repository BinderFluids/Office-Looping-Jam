using System;
using UnityEngine;
using Registry;

namespace Draggable
{
    public class DraggableCursor : MonoBehaviour
    {
        [SerializeField] private InputReader input; 
        [SerializeField] private float cursorSize = 10f;
        
        private void Update()
        {
            DraggableBehaviour target =
                Registry<DraggableBehaviour>
                    .Get(new Closest(cursorSize, input.MouseWorldPosition));
        }
    }
}