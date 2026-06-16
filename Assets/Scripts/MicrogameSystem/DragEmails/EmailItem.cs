using UnityEngine;
using Registry;
using UnityEditor.Rendering;

namespace MicrogameSystem.DragEmails
{
    public class EmailItem : MicrogameBehaviour<DragEmailsContext>
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] emailImages; 
        [SerializeField] private EmailType type;
        public EmailType Type => type;
        
        public Sprite GetImage() => emailImages[(int) type];

        protected override void Awake()
        {
            base.Awake();
            DragEmailsContext.Instance.AddBehaviour(this); 
        }

        public void Init()
        {
            type = (EmailType)Random.Range(0, emailImages.Length); 
            spriteRenderer.sprite = GetImage();
        }

        protected override void OnDestroy()
        {
            DragEmailsContext.Instance.RemoveBehaviour(this); 
        }
    }
}