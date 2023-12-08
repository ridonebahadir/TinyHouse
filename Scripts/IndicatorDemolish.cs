using UnityEngine;

namespace _Project.Scripts
{
    public class IndicatorDemolish : BaseIndicator, IInteractable
    {
        private CraneArea _craneArea;
        public Vector3 InteractPosition => transform.position;
        public bool CanInteract { get; set; } = true;

        public void Interact(InteractArgs args)
        {
            if (!CanInteract) return;
            
            SetCooldown(() =>
            {
                args.Worker.TryDemolish(_craneArea);
            });
        }

        public void StopInteract(InteractArgs args)
        {
            InterruptCooldown();
        }

        public override void InitArea(AreaController area)
        {
            base.InitArea(area);
            _craneArea = area.CraneArea;
        }
    }
}