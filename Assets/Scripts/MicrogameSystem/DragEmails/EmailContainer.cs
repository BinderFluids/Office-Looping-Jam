using UnityEngine;

namespace MicrogameSystem.DragEmails
{
    public class EmailContainer : MicrogameBehaviour<DragEmailsContext>
    {
        [SerializeField] private EmailType type;
        public EmailType Type => type;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<EmailItem>(out var email)) return;
            if (email.Type != type) ctx.Fail();
            
            Destroy(email.gameObject); 
        }
    }
}