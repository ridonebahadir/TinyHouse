using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts
{
    public class IndicatorCollectMoney : BaseIndicator, IInteractable
    {
        [SerializeField] private Transform slotParent;
        [SerializeField] private float moneySpawnInterval;
        [SerializeField] private float moneyCollectInterval = .1f;
        [SerializeField] private ResourceSo resourceSo;
        [SerializeField] private AudioClip withdrawMoneySound;

        private MoneyStash _stash;
        private WaitForSeconds _spawnIntervalWfs;
        private WaitForSeconds _moneyCollectIntervalWfs;

        public Vector3 InteractPosition { get; }

        private bool _canInteract;

        public bool CanInteract
        {
            get => _stash.GetStachCount() > 0;
            set => _canInteract = value;
        }

        protected override void Awake()
        {
            base.Awake();
            _stash = new MoneyStash();
            _spawnIntervalWfs = new WaitForSeconds(moneySpawnInterval);
            _moneyCollectIntervalWfs = new WaitForSeconds(moneyCollectInterval);
        }

        public override void InitArea(AreaController area)
        {
            base.InitArea(area);
            AssignedArea.TinyHouseCarController.OnTinyHouseCompleted += SpawnConcreteMoney;

            int defaultCount = ES3.Load($"{AssignedArea.AreaID}_MoneyStashCount", 0);
            _calculatedWorth = ES3.Load($"{AssignedArea.AreaID}_CalculatedWorth", 0);
            
            for (int i = 0; i < defaultCount; i++) SpawnConcreteMoney(1, _calculatedWorth);
        }

        private void OnDisable()
        {
            AssignedArea.TinyHouseCarController.OnTinyHouseCompleted -= SpawnConcreteMoney;
            
            if (AssignedArea != null && _stash != null)
            {
                ES3.Save($"{AssignedArea.AreaID}_MoneyStashCount", _stash.GetStachCount());
                ES3.Save($"{AssignedArea.AreaID}_CalculatedWorth", _calculatedWorth);
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (AssignedArea != null && _stash != null)
            {
                ES3.Save($"{AssignedArea.AreaID}_MoneyStashCount", _stash.GetStachCount());
                ES3.Save($"{AssignedArea.AreaID}_CalculatedWorth", _calculatedWorth);
            }
        }

        private void OnApplicationQuit()
        {
            if (AssignedArea != null && _stash != null)
            {
                ES3.Save($"{AssignedArea.AreaID}_MoneyStashCount", _stash.GetStachCount());
                ES3.Save($"{AssignedArea.AreaID}_CalculatedWorth", _calculatedWorth);
            }
        }

        private Vector3 GetSlotPositionAt(int idx)
        {
            return slotParent.GetChild(idx % slotParent.childCount).position;
        }

        private int _calculatedWorth;

        private void SpawnConcreteMoney(int rewardAmount, int worth)
        {
            if (_stash.GetStachCount() > 200) return;

            _calculatedWorth = (worth / rewardAmount);

            StartCoroutine(IE_ActivateMoneyBucket());

            IEnumerator IE_ActivateMoneyBucket()
            {
                for (int i = 0; i < rewardAmount; i++)
                {
                    GameObject moneyPiece =
                        ResourceManager.Instance.SpawnMoneyPiece(GetSlotPositionAt(_stash.GetStachCount()));
                    _stash.AddToStach(moneyPiece);
                    yield return _moneyCollectIntervalWfs;
                }
            }
        }

        public void Interact(InteractArgs args)
        {
            StartCoroutine(IE_CollectMoney());

            IEnumerator IE_CollectMoney()
            {
                if (CanInteract) SoundManager.Instance.PlaySound(withdrawMoneySound);
                while (CanInteract)
                {
                    GameObject stack = _stash.RemoveFromStach();
                    UIManager.Instance.SpawnResourceUIPiece(transform.position,
                        () => resourceSo.Increase(_calculatedWorth));
                    stack.transform.DOScale(Vector3.zero, .1f).OnComplete(() => stack.gameObject.SetActive(false));
                    yield return _moneyCollectIntervalWfs;
                }
            }
        }

        public void StopInteract(InteractArgs args)
        {
        }
    }
}