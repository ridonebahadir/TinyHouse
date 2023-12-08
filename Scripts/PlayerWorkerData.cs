using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts
{
    [CreateAssetMenu(menuName = "Data/Player Worker Data", order = 0)]
    public class PlayerWorkerData : ScriptableObject
    {
        [SerializeField] private string workerName;
        [SerializeField] private Sprite workerSprite;
        
        [Title("Hire Worker Stats")]
        [SerializeField] private int defaultMaxSize;
        [SerializeField] private float defaultSpeed;
        
        [Title("Upgrade Properties")] 
        [SerializeField] private Sprite stackSprite;
        [SerializeField] private Sprite movementSprite;

        public Sprite WorkerSprite => workerSprite;
        
        public string WorkerName => workerName;
        
        public Sprite StackSprite => stackSprite;

        public Sprite MovementSprite => movementSprite;

        public Action OnCurrentStackSizeChanged;
        public Action OnCurrentMoveSpeedChanged;
        public int CurrentStackSize
        {
            get => ES3.Load($"{name}_maxSize", defaultMaxSize);
            set
            {
                ES3.Save($"{name}_maxSize", value);
                OnCurrentStackSizeChanged?.Invoke();
            }
        }

        public float CurrentMoveSpeed
        {
            get => ES3.Load($"{name}_moveSpeed", defaultSpeed);
            set
            {
                ES3.Save($"{name}_moveSpeed", value);
                OnCurrentMoveSpeedChanged?.Invoke();
            }
        }
        
        public int CurrentStackSizeLevel
        {
            get => ES3.Load($"{name}_maxSizeLevel", 1);
            set => ES3.Save($"{name}_maxSizeLevel", value);
        }
        
        public int CurrentMovementSpeedLevel
        {
            get => ES3.Load($"{name}_moveSpeedLevel", 1);
            set => ES3.Save($"{name}_moveSpeedLevel", value);
        }

        public void UpgradeCurrentStackSize()
        {
            CurrentStackSize++;
            CurrentStackSizeLevel++;
        }

        public void UpgradeCurrentMoveSpeed()
        {
            CurrentMoveSpeed += .1f;
            CurrentMovementSpeedLevel++;
        }

    }
}