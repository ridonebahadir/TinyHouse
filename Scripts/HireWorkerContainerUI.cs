using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class HireWorkerContainerUI : MonoBehaviour
    {
        [SerializeField] private Image workerImage;
        [SerializeField] private TextMeshProUGUI workerName;
        [SerializeField] private TextMeshProUGUI workerCostText;
        [SerializeField] private TextMeshProUGUI currentStackText;
        [SerializeField] private TextMeshProUGUI currentSpeedText;
        [SerializeField] private TextMeshProUGUI currentLevelText;


        [SerializeField] private Button buyButton;
        [SerializeField] private Button upgradeButton;

        [SerializeField] private ResourceSo resource;

        private AIHireWorkerData _hireWorkerData;
        private HireWorkerBuildingUI _ownerUI;
        private bool _isInUpgradeStage;

        private static Action OnUpgraded;

        private void OnEnable()
        {
            buyButton.onClick.AddListener(BuyWorker);
            upgradeButton.onClick.AddListener(UpgradeWorkerOnClick);
            OnUpgraded += InitWorkerUI;
        }

        private void OnDisable()
        {
            buyButton.onClick.RemoveListener(BuyWorker);
            upgradeButton.onClick.RemoveListener(UpgradeWorkerOnClick);
            OnUpgraded -= InitWorkerUI;

        }

        public void InitData(AIHireWorkerData data, HireWorkerBuildingUI hireWorkerBuildingUI)
        {
            _ownerUI = hireWorkerBuildingUI;
            _hireWorkerData = data;

            SetUI();
        }
        
        public void SetUI()
        {
            InitWorkerUI();
            ActivateRequiredButton(buyButton.gameObject);

            if (_hireWorkerData.HasBoughtHireWorker)
            {
                // Bought worker; Now its to time upgrade it.
                _isInUpgradeStage = true;
                ActivateRequiredButton(upgradeButton.gameObject);
            } 
        }

        private void InitWorkerUI()
        {
            workerImage.sprite = _hireWorkerData.WorkerSprite;
            workerName.text = _hireWorkerData.WorkerName;
            currentStackText.text = _hireWorkerData.CurrentStackSize.ToString();
            currentSpeedText.text = $"{_hireWorkerData.CurrentMoveSpeed:0.0}";
            currentLevelText.text = $"LV {_hireWorkerData.HireWorkerLevel}";

            if (_isInUpgradeStage)
            {
                workerCostText.text = GetUpgradeCost().ToCurrency();
                
                if (!resource.HasEnoughAmount(GetUpgradeCost()))
                {
                    upgradeButton.interactable = false;
                    buyButton.interactable = false;
                }
                else
                {
                    upgradeButton.interactable = true;
                    buyButton.interactable = true;
                }
            }
            else
            {
                workerCostText.text = GetBuyCost().ToCurrency();
                
                if (!resource.HasEnoughAmount(GetBuyCost()))
                {
                    upgradeButton.interactable = false;
                    buyButton.interactable = false;
                }
                else
                {
                    upgradeButton.interactable = true;
                    buyButton.interactable = true;
                }
            }
        }

        private void BuyWorker()
        {
            if (resource.HasEnoughAmount(GetBuyCost()))
            {
                resource.Decrease(GetBuyCost());
                // Spawn workers
                _ownerUI.Building.SpawnWorker(_hireWorkerData);

                _isInUpgradeStage = true;
                // Bought worker; Now its to time upgrade it.
                ActivateRequiredButton(upgradeButton.gameObject);
                
                OnUpgraded?.Invoke();
                
                TutorialManager.Instance.SetTutorialPanel(_ownerUI.HiredWorkerTutorial, new[]
                {
                    "Well done! Your workers now collect and sell pieces!",
                    "They wont be able to demolish at first. Once you unlock a new area..",
                    "Workers will also demolish and acquire pieces.",
                    "Now its time to demolish again! Go to demolish indicator and..",
                    "DEMOLISH BUILDINGS!!"
                }, () =>
                {
                    var playerTransform = AreaManager.Instance.PlayerWorker.transform;
                    var demolishPos = AreaManager.Instance.DefaultArea.PieceDemolishArea.InteractPosition;
                    TutorialManager.Instance.SetTutorialLine(playerTransform, demolishPos);
                });
            }
        }

        private int GetBuyCost()
        {
            int currentLevel = _ownerUI.Building.AssignedArea.AreaLevel;
            return Formulas.GetWorkerCost(_hireWorkerData.HireWorkerLevel + _hireWorkerData.WorkerCostMultiplier,
                _ownerUI.Building.AssignedArea.OnBoardingLevel, currentLevel);
        }

        private int GetUpgradeCost()
        {
            int currentLevel = _ownerUI.Building.AssignedArea.AreaLevel;
            return Formulas.GetWorkerCost(_hireWorkerData.HireWorkerLevel + 1 + _hireWorkerData.WorkerCostMultiplier,
                _ownerUI.Building.AssignedArea.OnBoardingLevel,currentLevel);
        }

        private void UpgradeWorkerOnClick()
        {
            var upgradeCost = GetUpgradeCost();

            if (resource.HasEnoughAmount(upgradeCost))
            {
                resource.Decrease(upgradeCost);
                _hireWorkerData.UpgradeStats();
                OnUpgraded?.Invoke();
            }
        }

        private void ActivateRequiredButton(GameObject obj)
        {
            buyButton.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(false);
            obj.gameObject.SetActive(true);
        }
    }
}