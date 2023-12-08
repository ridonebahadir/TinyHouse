using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class IndicatorCollectDemolishPieces : BaseIndicator, IInteractable
    {
        [SerializeField] private float withdrawStackInterval = .1f;
        [SerializeField] private Transform slotParent;

        private WaitForSeconds _withdrawStackIntervalWfs;

        private DemolishPieceStash _stash;
        public DemolishPieceStash Stash => _stash;

        protected override void Awake()
        {
            _stash = new DemolishPieceStash();
            _withdrawStackIntervalWfs = new WaitForSeconds(withdrawStackInterval);
        }

        public override void InitArea(AreaController area)
        {
            base.InitArea(area);
            int defaultCount = ES3.Load($"{AssignedArea.AreaID}_StashCount", 5);

            for (int i = 0; i < defaultCount; i++) SpawnDemolishPiece(area.BrickSpawnChance);
        }

        public void SpawnDemolishPiece(float brickSpawnChance)
        {
            DemolishPiece piece = ResourceManager.Instance.SpawnDemolishPiece(GetSlotPositionAt(Stash.GetStachCount()));

            var randomValue = Random.value;

            if (brickSpawnChance >= randomValue)
            {
                piece.SetPieceDataByType(PieceType.Brick, 200);
            }
            else
            {
                piece.SetPieceDataByType(PieceType.Ytong, 400);
            }

            AddDemolishPieceToStach(piece);
        }

        private Vector3 GetSlotPositionAt(int idx)
        {
            return slotParent.GetChild(idx % slotParent.childCount).position;
        }

        public Vector3 InteractPosition => transform.position;

        private bool _canInteract;

        public bool CanInteract
        {
            get => Stash.GetStachCount() > 0;
            set => _canInteract = value;
        }

        public void Interact(InteractArgs args)
        {
            if (Stash.GetStachCount() <= 0)
            {
                Debug.LogWarning("Not available demolish pieces in: " + gameObject.name);
                return;
            }

            args.Worker.InteractRoutine = args.Worker.StartCoroutine(IE_WithdrawStacks());

            IEnumerator IE_WithdrawStacks()
            {
                while (args.Worker.CanAddToStack() && Stash.GetStachCount() > 0)
                {
                    args.Worker.AddStack(Stash.RemoveFromStach());
                    yield return _withdrawStackIntervalWfs;
                }

                if (!args.Worker.CanAddToStack())
                {
                    Debug.LogWarning("Player reached max stack!");
                }

                if (Stash.GetStachCount() < 0)
                {
                    Debug.LogWarning("All pieces withdrawn to the player.");
                }
            }
        }

        private void AddDemolishPieceToStach(DemolishPiece piece)
        {
            Stash.AddToStach(piece);
        }

        private void OnDisable()
        {
            ES3.Save($"{AssignedArea.AreaID}_StashCount", _stash.GetStachCount());
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (AssignedArea != null && _stash != null) ES3.Save($"{AssignedArea.AreaID}_StashCount", _stash.GetStachCount());
        }

        private void OnApplicationQuit()
        {
            ES3.Save($"{AssignedArea.AreaID}_StashCount", _stash.GetStachCount());
        }

        public void StopInteract(InteractArgs args)
        {
            //StopInteractRoutine();
            if (args.Worker != null) args.Worker.InterruptInteractRoutine();
        }
    }
}