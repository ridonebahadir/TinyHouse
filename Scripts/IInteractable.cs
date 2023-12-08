using UnityEngine;

namespace _Project.Scripts
{
    public class InteractArgs
    {
        public InteractArgs(BaseWorker worker)
        {
            Worker = worker;
        }
        public BaseWorker Worker { get; }
    }
    
    public interface IInteractable
    {
        public Vector3 InteractPosition { get; }
        public bool CanInteract { get; set;  }
        public void Interact(InteractArgs args);
        public void StopInteract(InteractArgs args);
    }
}