using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Transform targetTransform; // Assign the menu item here
        [SerializeField] private float floatAmplitude = 4f; // Pixels to move up/down
        [SerializeField] private float floatFrequency = 2f; // Speed of floating
        [SerializeField] private float scaleAmount = 1.05f; // How much to scale up
        [SerializeField] private float scaleSpeed = 3f; // Smooth scaling speed

        private bool isMouseOver = false;
        private Vector3 originalPosition;
        private Vector3 originalScale;

        void Start()
        {
            if (targetTransform == null)
                targetTransform = transform;

            originalPosition = targetTransform.localPosition;
            originalScale = targetTransform.localScale;
        }

        void Update()
        {
            if (isMouseOver)
            {
                // Floating effect
                float newY = originalPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
                targetTransform.localPosition = new Vector3(originalPosition.x, newY, originalPosition.z);

                // Smooth scale up
                targetTransform.localScale = Vector3.Lerp(targetTransform.localScale, originalScale * scaleAmount, Time.deltaTime * scaleSpeed);
            }
            else
            {
                // Return to original position
                targetTransform.localPosition = Vector3.Lerp(targetTransform.localPosition, originalPosition, Time.deltaTime * scaleSpeed);

                // Return to original scale
                targetTransform.localScale = Vector3.Lerp(targetTransform.localScale, originalScale, Time.deltaTime * scaleSpeed);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isMouseOver = false;
        }
    }
}