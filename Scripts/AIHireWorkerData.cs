using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    [CreateAssetMenu(menuName = "Data/Hire Worker Data", order = 0)]
    public class AIHireWorkerData : ScriptableObject
    {
        [Title("General Settings")] [SerializeField]
        private AIWorker workerPrefab;

        [SerializeField] private string workerName;
        [SerializeField] private Sprite workerSprite;

        [Title("Hire Worker Stats")] 
        [SerializeField] private int defaultMaxSize;
        [SerializeField] private float defaultSpeed;

        [Title("Upgrade")] [SerializeField]
        private int workerCostMultiplier = 0;

        public AIWorker WorkerPrefab => workerPrefab;
        public Sprite WorkerSprite => workerSprite;
        public string WorkerName => workerName;

        public bool HasBoughtHireWorker
        {
            get => ES3.Load($"{name}_hasBought", false);
            set => ES3.Save($"{name}_hasBought", value);
        }

        public int CurrentStackSize
        {
            get => ES3.Load($"{name}_maxSize", defaultMaxSize);
            set => ES3.Save($"{name}_maxSize", value);
        }

        public float CurrentMoveSpeed
        {
            get => ES3.Load($"{name}_moveSpeed", defaultSpeed);
            set => ES3.Save($"{name}_moveSpeed", value);
        }

        public int HireWorkerLevel
        {
            get => ES3.Load($"{name}hireWorkerLevel", 1);
            set => ES3.Save($"{name}hireWorkerLevel", value);
        }
        
        public void UpgradeStats()
        {
            CurrentStackSize++;
            CurrentMoveSpeed += .1f;
            HireWorkerLevel++;
            HiredWorkerInstance.ApplyStats();
        }

        public AIWorker HiredWorkerInstance { get; private set; }

        // Return multiplier for workers
        public int WorkerCostMultiplier => workerCostMultiplier - 1;

        public AIWorker SpawnWorker(Vector3 position, AreaController currentArea)
        {
            HasBoughtHireWorker = true;
            HiredWorkerInstance = Instantiate(workerPrefab, position, Quaternion.identity);
            HiredWorkerInstance.InitAI(this, currentArea);
            return HiredWorkerInstance;
        }
    }
}