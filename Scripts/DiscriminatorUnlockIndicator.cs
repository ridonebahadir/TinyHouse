using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    [Serializable]
    public class PieceUpgradeData
    {
        public PieceData[] discriminatorPieceData;
    }

    public class DiscriminatorUnlockIndicator : BaseUnlockIndicator
    {
        [SerializeField] private string saveID;
        [SerializeField] private int discriminatorSpawnCount = 2;

        [SerializeField] private Transform startAreaPoint;
        [SerializeField] private Transform endAreaPoint;
        [SerializeField] private Transform[] discriminatorSpawnPoints;

        [FormerlySerializedAs("discriminationData")] [SerializeField]
        private PieceUpgradeData[] pieceUpgradeData;

        public PieceType[] requiredPiecesToDiscriminate;


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

            int[] costIdx = new[] { 0, 2 };
            
            var spawnedStart =
                Controller.SpawnDiscriminatorStart(startAreaPoint.position, requiredPiecesToDiscriminate);
            var spawnedEnd = Controller.SpawnDiscriminatorEnd(endAreaPoint.position);

            float value = -1f;
            for (int i = 0; i < discriminatorSpawnCount; i++)
            {
                var building = Controller.SpawnDiscriminator(pieceUpgradeData[i].discriminatorPieceData,
                    requiredPiecesToDiscriminate[i], discriminatorSpawnPoints[i].position, costIdx[i],
                    $"{saveID}_{i}", spawnedStart, spawnedEnd);
                building.transform.localScale = new Vector3(building.transform.localScale.x * value, building.transform.localScale.y, building.transform.localScale.z);
                building.BuildingIndicatorPoint.localScale = new Vector3(building.BuildingIndicatorPoint.localScale.x * value, building.BuildingIndicatorPoint.localScale.y, building.BuildingIndicatorPoint.localScale.z);
                value *= -1f;
            }

            UnlockedIndicatorTargetObject = true;
            gameObject.SetActive(false);
        }

        public override void StopInteract(InteractArgs args)
        {
            InterruptCooldown();
        }
    }
}