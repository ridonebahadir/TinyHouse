using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class DiscriminatorUpgradeUI : BaseBuildingUI
    {
        [SerializeField] private Button buyButton;
        [SerializeField] private Image pieceTypeImage;
        [SerializeField] private TextMeshProUGUI typeText;
        [SerializeField] private TextMeshProUGUI buttonText;
        
        public Discriminator Discriminator { get; private set; }

        public void InitUI(Discriminator dc)
        {
            Discriminator = dc;
            InitElements();
        }

        public override void Open()
        {
            base.Open();
            buyButton.onClick.AddListener(Upgrade);
        }

        private void Upgrade()
        {
            if (Discriminator.IncreaseUpgradeIndex())
            {
                InitElements();
            }
        }

        public override void Close()
        {
            base.Close();
            buyButton.onClick.RemoveListener(Upgrade);
        }

        private void InitElements()
        {
            buyButton.gameObject.SetActive(true); 
            
            // TODO: Set image later on.
            if (Discriminator.CanUpgrade())
            {
                SetTypeText();
            }
            else
            {
                typeText.text = "Completed!";
                buyButton.gameObject.SetActive(false);
            }
        }

        private void SetTypeText()
        {
            buttonText.text = Discriminator.GetResourceCost().ToCurrency();
            typeText.text = Discriminator.GetNextUpgradeName();
        }
    }
}