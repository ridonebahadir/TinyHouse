using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseCraneUpgradeContainer : MonoBehaviour
{
    [SerializeField] Image upgradeTypeImage;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button watchAdButton;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TutorialData craneUpgradedData;

    [Title("Upgrade Data")] 
    [SerializeField] protected ResourceSo resource;

    protected CraneStats _stats;

    private UpgradeCraneBuildingUI _ui;
    private static Action OnUpgraded;

    public void InitContainer(UpgradeCraneBuildingUI ui)
    {
        _ui = ui;
        _stats = _ui.Building.AssignedArea.Stats;
    }

    private void Start()
    {
        upgradeButton.onClick.AddListener(OnClickedUpgradeButton);
        watchAdButton.onClick.AddListener(OnClickedWatchADButton);
    }
    

    private void OnEnable()
    {
        OnUpgraded += UpdateUI;
    }

    private void OnDisable()
    {
        OnUpgraded -= UpdateUI;
        upgradeButton.onClick.RemoveListener(OnClickedUpgradeButton);
        watchAdButton.onClick.RemoveListener(OnClickedWatchADButton);
    }

    public void ButtonInteractable(bool interactableBool)
    {
        upgradeButton.interactable = interactableBool;
    }

    private void SetDescriptionText()
    {
        descriptionText.text = GetDescriptionText();
    }

    private void SetUpgradeCost()
    {
        upgradeCostText.text = $"${GetUpgradeCost().ToCurrency()}";
    }

    private void SetLevelText()
    {
        levelText.text = $"LV {GetLevel()}";
    }

    private void SetUpgradeImage()
    {
        upgradeTypeImage.sprite = GetUpgradeSprite();
    }

    protected abstract Sprite GetUpgradeSprite();
    protected abstract int GetUpgradeCost();
    protected abstract int GetLevel();
    protected abstract int GetMaxLevel();
    protected abstract string GetDescriptionText();

    protected void OnClickedUpgradeButton()
    {
        TutorialManager.Instance.SetTutorialPanel(craneUpgradedData, new[]
            {
                "Good job! Your crane is now upgraded!..",
                "You have still remaining money. Upgrade your crane more!",
            }, () => { _ui.ActivateCloseButton(true); }, () => { TutorialManager.Instance.SetFingerObj(false); }
        );
        
        if (resource.HasEnoughAmount(GetUpgradeCost()))
        {
            // Decrease
            resource.Decrease(GetUpgradeCost());

            // Play sound
            SoundManager.Instance.PlayPurchaseSound();

            // Upgrade stats by container type
            UpgradeStats();

            // Notify upgrades
            OnUpgraded?.Invoke();
        }
    }

    private void OnClickedWatchADButton()
    {
        AdsManager.Instance.TryShowRewarded(() =>
        {
            UpgradeStats();
            OnUpgraded?.Invoke();
        });
    }

    protected abstract void UpgradeStats();

    public void UpdateUI()
    {
        
        SetUpgradeImage();
        SetUpgradeCost();
        SetDescriptionText();
        SetLevelText();

        if (!resource.HasEnoughAmount(GetUpgradeCost()))
        {
            // Doesnt have enough money..Maybe ADS?
            upgradeButton.gameObject.SetActive(false);
            watchAdButton.gameObject.SetActive(true);
            watchAdButton.interactable = true;
        }
        else
        {
            watchAdButton.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(true);
            upgradeButton.interactable = true;
        }

        if (GetLevel() >= GetMaxLevel())
        {
            upgradeCostText.text = "";
            levelText.text = "MAX";
            watchAdButton.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(false);
            
        }
    }
}