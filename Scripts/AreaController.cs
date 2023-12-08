using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class AreaController : MonoBehaviour
    {
        [Title("Level of the Area")]
        [SerializeField] private int areaLevel;
        
        [Title("Demolish Piece Chances")] 
        [SerializeField] private float brickSpawnChance;


        [Title("Crane Stats for All")]
        
        [SerializeField] private CraneStats stats;
        [Title("")]
        [SerializeField] private bool onBoardingLevel;

        [Title("Controllers In Area")]
        [SerializeField] private BuildingController buildingController;
        [SerializeField] private TinyHouseCarController tinyHouseCarController;

        [Title("Interactable Job Indicators")] 
        [SerializeField] private IndicatorCollectDemolishPieces collectArea;
        [SerializeField] private IndicatorSellDemolishPieces sellArea;
        [SerializeField] private IndicatorCollectMoney collectMoneyArea;
        [SerializeField] private IndicatorDemolish demolishArea;

        [Title("Unlock Indicators")] 
        [SerializeField] private UnlockNewAreaIndicator unlockAreaIndicator;
        [SerializeField] private HireWorkerUnlockIndicator hireWorkerBuildingUnlockIndicator;
        [SerializeField] private UpgradeCraneUnlockIndicator upradeCraneBuildingUnlockIndicator;
        [SerializeField] private DiscriminatorUnlockIndicator[] discriminatorUnlockIndicators;
        [SerializeField] private PlayerGymUnlockIndicator gymUnlockIndicator;
        

        [Title("Crane Stats")] [SerializeField]
        private CraneArea craneArea;

        [Title("Unlock Settings")] 
        [SerializeField] private float areaXClampValue = 27f;
        [SerializeField] private Gate unlockGate;
        

        [Title("Area ID")] [SerializeField] private string areaID;

        // Piece Collect Indicator near Demolish
        public IInteractable PieceCollectArea => collectArea;

        // Sell area for pieces
        public IInteractable PieceSellArea => sellArea;

        // Demolish area to achieve piece
        public IInteractable PieceDemolishArea => demolishArea;

        // Discriminator Start Indicator
        public IInteractable PieceDiscriminatorStartArea;

        // Discriminator End Indicator
        public IndicatorDiscriminatorEnd PieceDiscriminatorEndArea { get; set; }

        // Tiny house reference for Indicators
        public TinyHouseCarController TinyHouseCarController => tinyHouseCarController;

        // Currently active workers in area
        public List<AIWorker> ActiveWorkers { get; private set; } = new List<AIWorker>();

        // Area's current Clamp value
        public float AreaXClampValue => areaXClampValue;

        // Currently active car in the area
        public TinyHouseCar CurrentCarInArea => tinyHouseCarController.CurrentCar;

        // Area ID for saving stuff
        public string AreaID => areaID;

        public bool OnBoardingLevel => onBoardingLevel;

        public Gate UnlockGate => unlockGate;

        public CraneArea CraneArea => craneArea;

        public int AreaLevel => areaLevel;

        public CraneStats Stats => stats;

        public BuildingController BuildingController => buildingController;

        public float BrickSpawnChance => brickSpawnChance;


        private void Start()
        {
            InitAllAreaElements();
            LoadData();
            tinyHouseCarController.InitializeTinyHouseController();
            CraneArea.Stats.ResetCollectedPieces();
        }

        private void OnEnable()
        {
            CraneArea.OnClickedExitArea += FetchDemolishedPieces;
        }

        
        private void OnDisable()
        {
            CraneArea.OnClickedExitArea -= FetchDemolishedPieces;
        }

        private void LoadData()
        {
            // Set player default value
            AreaManager.Instance.SetPlayerClampValues(AreaXClampValue);

            // Interaction indicators
            sellArea.TryLoadIndicatorData();
            collectArea.TryLoadIndicatorData();
            sellArea.TryLoadIndicatorData();
            collectMoneyArea.TryLoadIndicatorData();
            demolishArea.TryLoadIndicatorData();

            // Building indicators & they also spawn building & regarding pieces etc.
            gymUnlockIndicator.TryLoadIndicatorData(BuildingController);
            hireWorkerBuildingUnlockIndicator.TryLoadIndicatorData(BuildingController);
            upradeCraneBuildingUnlockIndicator.TryLoadIndicatorData(BuildingController);

            foreach (var discriminatorUnlockIndicator in discriminatorUnlockIndicators) discriminatorUnlockIndicator.TryLoadIndicatorData(BuildingController);
            unlockAreaIndicator.TryLoadIndicatorData(BuildingController, true);
        }

        private void InitAllAreaElements()
        {
            Stats.AssignedArea = this;
            foreach (var areaElement in GetComponentsInChildren<IAreaElement>(true)) areaElement?.InitArea(this);
        }

        public void AddWorker(AIWorker worker)
        {
            ActiveWorkers.Add(worker);
        }

        public AIWorker GetRandomWorker()
        {
            return ActiveWorkers[Random.Range(0, ActiveWorkers.Count)];
        }

        private void FetchDemolishedPieces()
        {
            int collectedPieceCount = Mathf.Min(450, CraneArea.Stats.CollectedPieces);
            
            Debug.Log("Fetching demolish pieces: " + CraneArea.Stats.CollectedPieces);

            StartCoroutine(IE_SpawnPieces());

            IEnumerator IE_SpawnPieces()
            {
                for (int i = 0; i < collectedPieceCount; i++)
                {
                    TransferPiecesToCollectIndicator(BrickSpawnChance);
                    yield return null;
                }

                CraneArea.Stats.ResetCollectedPieces();
            }
        }

        public void TransferPiecesToCollectIndicator(float brickChance)
        {
            collectArea.SpawnDemolishPiece(brickChance);
        }

        public void DisableDemolishIndicatorCollision()
        {
            demolishArea.DisableCollider();
        }

        public void ActivateHireWorkerIndicator()
        {
            hireWorkerBuildingUnlockIndicator.ActivateIndicator(BuildingController);
        }

        public PlayerGymUnlockIndicator GetPlayerGymIndicator()
        {
            return gymUnlockIndicator;
        }

        public HireWorkerUnlockIndicator GetHireWorkerIndicator()
        {
            return hireWorkerBuildingUnlockIndicator;
        }

        public BaseUnlockIndicator GetUpgradeCraneIndicator()
        {
            return upradeCraneBuildingUnlockIndicator;
        }

        public void ActivateUpgradeCraneIndicator()
        {
            GetUpgradeCraneIndicator().ActivateIndicator(BuildingController);
        }

        public void ActivatePlayerGymIndicator()
        {
            GetPlayerGymIndicator().ActivateIndicator(BuildingController);
        }

        public void UnlockArea()
        {
            gameObject.SetActive(true);
        }
    }
}