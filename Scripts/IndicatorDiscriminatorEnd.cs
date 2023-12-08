using System.Collections;
using UnityEngine;

namespace _Project.Scripts
{
    public class IndicatorDiscriminatorEnd : BaseIndicator, IInteractable
    {
        [SerializeField] private float withdrawStackInterval = .1f;
        [SerializeField] private Transform slotParent;

        private WaitForSeconds _withdrawStackIntervalWfs;

        private DemolishPieceStash _stash;

        protected override void Awake()
        {
            _stash = new DemolishPieceStash();
            _withdrawStackIntervalWfs = new WaitForSeconds(withdrawStackInterval);
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

        public DemolishPieceStash Stash => _stash;


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

        public override void InitArea(AreaController area)
        {
            base.InitArea(area);
            area.PieceDiscriminatorEndArea = this;
        }

        public void AddDemolishPieceToStach(DemolishPiece piece)
        {
            piece.Jump(GetSlotPositionAt(Stash.GetStachCount()));
            Stash.AddToStach(piece);
        }

        public void StopInteract(InteractArgs args)
        {
            // StopInteractRoutine();
                        if (args.Worker != null) args.Worker.InterruptInteractRoutine();
        }
    }
}