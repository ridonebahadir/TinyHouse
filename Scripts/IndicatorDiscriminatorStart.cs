using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts
{
    public class IndicatorDiscriminatorStart : BaseIndicator, IInteractable
    {
        [SerializeField] private float sellInterval;
        [SerializeField] private Transform slotParent;

        private WaitForSeconds _sellIntervalWfs;
        private DemolishPieceStash _stash;

        public PieceType[] RequiredTypes { get; set; }
        
        protected override void Awake()
        {
            base.Awake();
            _stash = new DemolishPieceStash();
            _sellIntervalWfs = new WaitForSeconds(sellInterval);
        }

        public DemolishPiece RemoveStash()
        {
            return Stash.RemoveFromStach();
        }

        public DemolishPiece GetStash()
        {
            return Stash.GetPieceFromStash();
        }
        
        private Vector3 GetSlotPositionAt(int idx)
        {
            return slotParent.GetChild(idx % slotParent.childCount).position;
        }

        public Vector3 InteractPosition => transform.position;
        public bool CanInteract { get; set; }

        public DemolishPieceStash Stash => _stash;

        public WaitForSeconds SellIntervalWfs => _sellIntervalWfs;

        public void Interact(InteractArgs args)
        {
            args.Worker.InteractRoutine = args.Worker.StartCoroutine(IE_SellPieces());

            IEnumerator IE_SellPieces()
            {
                while (args.Worker.StackCount > 0)
                {
                    Debug.Log(RequiredTypes == null);
                    if (!args.Worker.CanDiscriminate(RequiredTypes)) yield break;
                    
                    DemolishPiece stack = args.Worker.RemoveStack();
                    LoadToStash(stack);
                    yield return SellIntervalWfs;
                }
            }
        }

        public override void InitArea(AreaController area)
        {
            base.InitArea(area);
            area.PieceDiscriminatorStartArea = this;
        }

        private void LoadToStash(DemolishPiece stack)
        {
            stack.transform.SetParent(null);
            stack.Jump(GetSlotPositionAt(Stash.GetStachCount()));
            Stash.AddToStach(stack);
        }

        public void StopInteract(InteractArgs args)
        {
            // StopInteractRoutine();
            if (args.Worker != null) args.Worker.InterruptInteractRoutine();
        }
    }
}