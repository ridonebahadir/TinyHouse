using UnityEngine;

namespace _Project.Scripts
{
    public class PlayerGymUnlockIndicator : BaseUnlockIndicator
    {
        [SerializeField] private Transform gymBuildingSpawnPoint;

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
            Controller.SpawnPlayerGym(gymBuildingSpawnPoint.position);
            UnlockedIndicatorTargetObject = true;
            gameObject.SetActive(false);
        }

        public override void StopInteract(InteractArgs args)
        {
            InterruptCooldown();
        }
    }
}