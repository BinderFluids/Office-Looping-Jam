using System.Collections.Generic;
using System.Linq;
using Registry;

namespace MicrogameSystem.DragEmails
{
    public class DragEmailsContext : MicrogameContext<DragEmailsContext>
    {
        protected override void OnStartMicrogame()
        {
            Registry<EmailItem>._onItemRemovedNoArgs += CheckEmailsSortedCorrectly; 
        }

        void CheckEmailsSortedCorrectly()
        {
            if (Registry<EmailItem>.Count > 0) return;
            Succeed();
        }

        protected override void OnEndMicrogame()
        {
            Registry<EmailItem>._onItemRemovedNoArgs -= CheckEmailsSortedCorrectly; 
        }
    }
}