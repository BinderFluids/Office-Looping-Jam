namespace MicrogameSystem.DragEmails
{
    public class DragEmailsContext : MicrogameContext<DragEmailsContext>
    {
        protected override void OnStart()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnEnd()
        {
            throw new System.NotImplementedException();
        }
    }

    public class EmailDraggable : MicrogameBehaviour<DragEmailsContext>
    {
        
        
        public override void OnMicrogameUpdate(float dt)
        {
            
        }
    }
}