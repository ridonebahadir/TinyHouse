using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class PlayerGymContainerUI : MonoBehaviour
    {
        [SerializeField] private Image workerImage;
        [SerializeField] private TextMeshProUGUI workerName;

        [Title("Buttons")] [SerializeField] private Button stackSizeUpgradeButton;
        [SerializeField] private Button movementSpeedUpgradeButton;

        [Title("Stack Upgrade")] 
        [SerializeField] private TextMeshProUGUI stackSizeText;
        [SerializeField] private TextMeshProUGUI stackCostText;

        [SerializeField] private Image stackImage;

        [Title("Movement Upgrade")] 
        [SerializeField] private TextMeshProUGUI movementText;
        [SerializeField] private TextMeshProUGUI movementCostText;

        [SerializeField] private Image movementImage;

        [SerializeField] private ResourceSo resource;
        private PlayerWorkerData _data;
        private PlayerGymBuildingUI _ownerUI;

        public static Action OnUpgraded;
        private void OnEnable()
        {
            stackSizeUpgradeButton.onClick.AddListener(UpgradeStackSize);
            movementSpeedUpgradeButton.onClick.AddListener(UpgradeMovementSpeed);
            OnUpgraded += InitUI;
        }

        private void OnDisable()
        {
            stackSizeUpgradeButton.onClick.RemoveListener(UpgradeStackSize);
            movementSpeedUpgradeButton.onClick.RemoveListener(UpgradeMovementSpeed);
            OnUpgraded -= InitUI;
        }

        public void InitData(PlayerWorkerData data, PlayerGymBuildingUI gymUI)
        {
            _ownerUI = gymUI;
            _data = data;
            InitUI();
        }

        private int movementSpeedCost;
        int stackCost;
        
        public void InitUI()
        {
            /*workerImage.sprite = _data.WorkerSprite;
            workerName.text = _data.WorkerName;*/

            movementSpeedCost = (int)Formulas.GetPlayerUpgradeCost(_data.CurrentMovementSpeedLevel);
            stackCost = (int)Formulas.GetPlayerUpgradeCost(_data.CurrentStackSizeLevel);
            
            
            stackSizeText.text = $"{_data.CurrentStackSize}";
            stackCostText.text = stackCost.ToCurrency();

            movementText.text = $"{_data.CurrentMoveSpeed:0.0}";
            movementCostText.text = movementSpeedCost.ToCurrency();

            if (!resource.HasEnoughAmount(movementSpeedCost))
            {
                movementSpeedUpgradeButton.interactable = false;
            }
            else
            {
                movementSpeedUpgradeButton.interactable = true;
            }
            
            if (!resource.HasEnoughAmount(stackCost))
            {
                stackSizeUpgradeButton.interactable = false;
            }
            else
            {
                stackSizeUpgradeButton.interactable = true;
            }

            /*stackImage.sprite = _data.StackSprite;
            movementImage.sprite = _data.MovementSprite;*/
        }

        private void UpgradeStackSize()
        {
            if (resource.HasEnoughAmount(stackCost))
            {
                SoundManager.Instance.PlayPurchaseSound();
                resource.Decrease(stackCost);
                _data.UpgradeCurrentStackSize();
                OnUpgraded?.Invoke();
            }
        }

        private void UpgradeMovementSpeed()
        {

            if (resource.HasEnoughAmount(movementSpeedCost))
            {
                SoundManager.Instance.PlayPurchaseSound();
                resource.Decrease(movementSpeedCost);
                _data.UpgradeCurrentMoveSpeed();
                OnUpgraded?.Invoke();
            }
        }
    }
}