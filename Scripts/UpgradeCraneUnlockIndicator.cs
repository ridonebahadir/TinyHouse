using UnityEngine;

namespace _Project.Scripts
{
    public class UpgradeCraneUnlockIndicator : BaseUnlockIndicator
    {
        [SerializeField] private Transform craneBuildingSpawnPoint;

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
            Controller.SpawnUpgradeCraneBuilding(craneBuildingSpawnPoint.position);
            UnlockedIndicatorTargetObject = true;
            gameObject.SetActive(false);
        }

        public override void StopInteract(InteractArgs args)
        {
            InterruptCooldown();
        }
    }
}