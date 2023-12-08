using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace _Project.Scripts
{
    public class IndicatorSellDemolishPieces : BaseIndicator, IInteractable
    {
        [SerializeField] private float sellInterval;
        [SerializeField] private Transform slotParent;

        // Routine settings
        private WaitForSeconds _sellIntervalWfs;

        // Stash
        private DemolishPieceStash _stash;

        // Current car
        private TinyHouseCar tinyHouseCar;


        protected override void Awake()
        {
            base.Awake();

            _stash = new DemolishPieceStash();
            _sellIntervalWfs = new WaitForSeconds(sellInterval);
        }

        public override void InitArea(AreaController area)
        {
            base.InitArea(area);
        }

        public override void Activate()
        {
            base.Activate();

            if (AssignedArea.CurrentCarInArea != null) InitCarOnSpawn(AssignedArea.CurrentCarInArea);
            AssignedArea.TinyHouseCarController.OnTinyHouseCarSpawned += InitCarOnSpawn;
        }

        private void OnDisable()
        {
            AssignedArea.TinyHouseCarController.OnTinyHouseCarSpawned -= InitCarOnSpawn;
        }

        private void InitCarOnSpawn(TinyHouseCar obj)
        {
            tinyHouseCar = obj;
            tinyHouseCar.OnArrived += OnCarArrived_LoadFromStashToCar;
        }

        private Vector3 GetSlotPositionAt(int idx)
        {
            return slotParent.GetChild(idx % slotParent.childCount).position;
        }

        public Vector3 InteractPosition => transform.position;

        public bool CanInteract { get; set; }

        public void Interact(InteractArgs args)
        {
            // Eğer tiny house car available değilse, player'in stacklerini
            args.Worker.InteractRoutine = args.Worker.StartCoroutine(IE_SellStacks());

            IEnumerator IE_SellStacks()
            {
                // playerda stack olduğu ve araç available ise; direkt olarak arabaya yükle.
                // Eğer playerda stack varsa ve araç available değil ise, zemini doldur. 
                // Araç vardığında da tekrar gerekli kontrolleri yap;

                while (args.Worker.StackCount > 0)
                {
                    // On every sell inverval
                    DemolishPiece stack = args.Worker.RemoveStack();

                    if (tinyHouseCar == null || !tinyHouseCar.IsAvailable)
                    {
                        Debug.LogWarning($"{args.Worker.name} is loading to the stash on sell indicator");
                        LoadToStash(stack);
                    }
                    else if (tinyHouseCar.IsAvailable)
                    {
                        Debug.LogWarning($"{args.Worker.name} is loading to the car");
                        LoadToCar(stack);
                    }

                    yield return _sellIntervalWfs;
                }
            }
        }


        private void OnCarArrived_LoadFromStashToCar()
        {
            StartCoroutine(IE_LoadToCar());

            IEnumerator IE_LoadToCar()
            {
                while (_stash.HasStach() && tinyHouseCar.IsAvailable)
                {
                    DemolishPiece stack = _stash.RemoveFromStach();
                    LoadToCar(stack);
                    yield return _sellIntervalWfs;
                }

                tinyHouseCar.OnArrived -= OnCarArrived_LoadFromStashToCar;
            }
        }

        private void LoadToStash(DemolishPiece stack)
        {
            stack.transform.SetParent(null);
            stack.Jump(GetSlotPositionAt(_stash.GetStachCount()));
            _stash.AddToStach(stack);
        }

        private void LoadToCar(DemolishPiece stack)
        {
            stack.transform.SetParent(null);
            stack.Jump(tinyHouseCar.DemolishPieceArrivePoint.position, () => stack.gameObject.SetActive(false));
            tinyHouseCar.LoadUpCar(stack.PieceWorth);
        }

        // This is where a car arrived and load to the car directly from indicator stash.


        public void StopInteract(InteractArgs args)
        {
            if (args.Worker != null) args.Worker.InterruptInteractRoutine();
        }
    }
}