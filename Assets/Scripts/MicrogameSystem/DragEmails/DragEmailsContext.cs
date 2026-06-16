using System.Collections.Generic;
using System.Linq;
using Registry;

namespace MicrogameSystem.DragEmails
{
    public class DragEmailsContext : MicrogameContext<DragEmailsContext>
    {
        protected override void OnStartMicrogame()
        {
            OnAddBehaviour.onEventNoArgs += CheckEmailsSortedCorrectly; 
        }

        void CheckEmailsSortedCorrectly()
        {
            if (Behaviours.Any(b => b is EmailItem)) return;
            Succeed();
        }

        protected override void OnEndMicrogame()
        {
            OnAddBehaviour.onEventNoArgs -= CheckEmailsSortedCorrectly; 
        }
    }
}