using Lofelt.NiceVibrations;
using UnityEngine;

namespace _Project.Scripts
{
    public class UnlockNewAreaIndicator : BaseUnlockIndicator
    {
        [SerializeField] private int levelIdx;

        public override void Interact(InteractArgs args)
        {
            base.Interact(args);

            if (!CanAfford()) return;

            SetCooldown(UnlockArea);
        }

        private void UnlockArea()
        {
            Pay();
            OnInteractCallback();
        }

        protected override void OnInteractCallback()
        {
            base.OnInteractCallback();

            UnlockedIndicatorTargetObject = true;

            AreaManager.Instance.UnlockNextArea(levelIdx);
            AssignedArea.UnlockGate.Unlock();
            AssignedArea.DisableDemolishIndicatorCollision();
            if (AssignedArea.BuildingController.CurrentCraneBuildingInArea != null) AssignedArea.BuildingController.CurrentCraneBuildingInArea.gameObject.SetActive(false);
            
            if (AssignedArea.BuildingController.CurrentHireBuildingInArea != null)
            {
                var demolisher = AssignedArea.BuildingController.CurrentHireBuildingInArea.SpawnDemolisher(AssignedArea);
                demolisher.CreateDemolishStates();
            }
            else
            {
                // Spawn from prefab
                var demolisher = AssignedArea.BuildingController.HireWorkerBuildingPrefab.SpawnDemolisher(AssignedArea);
                demolisher.CreateDemolishStates();
            }
            //

            gameObject.SetActive(false);
        }

        public override void StopInteract(InteractArgs args)
        {
            base.StopInteract(args);
            InterruptCooldown();
        }
    }
}