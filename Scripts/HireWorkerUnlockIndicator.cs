using System.Collections;
using UnityEngine;

namespace _Project.Scripts
{
    public class HireWorkerUnlockIndicator : BaseUnlockIndicator
    {
        [SerializeField] private Transform hireWorkerBuildingSpawnPoint;

        public override void Interact(InteractArgs args)
        {
            if (!CanAfford()) return;

            SetCooldown(() =>
            {
                Pay();
                OnInteractCallback();
            });
        }

        protected override void OnInteractCallback()
        {
            base.OnInteractCallback();
            Controller.SpawnHireWorkerBuilding(hireWorkerBuildingSpawnPoint.position);
            UnlockedIndicatorTargetObject = true;
            gameObject.SetActive(false);
        }

        public override void StopInteract(InteractArgs args)
        {
            InterruptCooldown();
        }
    }
}