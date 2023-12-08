using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.Scripts
{
    [Serializable]
    public struct UnlockRequirements
    {
        public ResourceSo requiredResource;
        public int cost;

        public bool CanAffordCost() => requiredResource.HasEnoughAmount(cost);
        public void Pay() => requiredResource.Decrease(cost);
    }
    
    public abstract class BaseUnlockIndicator : BaseIndicator, IInteractable
    {
        [SerializeField] private AudioClip unlockSound;
        [SerializeField] protected UnlockRequirements unlockRequirements;
        [SerializeField] protected TextMeshProUGUI costText;
        
        public Vector3 InteractPosition { get; }
        public bool CanInteract { get; set; }

        protected BuildingController Controller;
        private void InitializeController(BuildingController manager)
        {
            Controller = manager;
        }

        private Sequence seq;
        public BaseUnlockIndicator ActivateIndicator(BuildingController controller)
        {
            gameObject.SetActive(true);
            InitializeController(controller);
            UnlockedIndicator = true;

            /*var initialScale = transform.localScale;
            transform.localScale = Vector3.zero;

             seq = DOTween.Sequence();
            seq.SetDelay(2f).Append(transform.DOScale(initialScale, .3f));*/
            
            
            return this;
        }

        private void OnDisable()
        {
            transform.DOKill();
            
            if (seq != null)
            {
                seq?.Kill();
                seq = null;
            }   
        }

        public void TryLoadIndicatorData(BuildingController controller, bool shouldUnlockBuilding = true)
        {
            if (UnlockedIndicator)
            {
                ActivateIndicator(controller);

                if (shouldUnlockBuilding) if (UnlockedIndicatorTargetObject) OnInteractCallback();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        protected virtual void OnInteractCallback()
        {
            if (AdsManager.Instance != null) AdsManager.Instance.TryShowInterstitial();
            SoundManager.Instance.PlaySound(unlockSound);
        }

        protected override void Start()
        {
            base.Start();
            costText.text = $"${unlockRequirements.cost.ToCurrency()}";
        }

        protected bool CanAfford()
        {
            return unlockRequirements.CanAffordCost();
        }

        protected void Pay()
        {
            unlockRequirements.Pay();
        }
        
        public virtual void Interact(InteractArgs args)
        {
        }
        public virtual void StopInteract(InteractArgs args)
        {
        }
    }
}