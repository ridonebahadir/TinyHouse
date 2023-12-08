using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class UIManager : SingletonClass.Singleton<UIManager>
    {
        [SerializeField] private Transform resourceUIPosition;
        [SerializeField] private Canvas gameplayCanvas;
        [SerializeField] private GameObject singleMoneyPiecePrefab;

        [Title("Building UI")] 
        [SerializeField] private HireWorkerBuildingUI hireWorkerUI;
        [SerializeField] private UpgradeCraneBuildingUI craneUpgradeUI;
        [SerializeField] private DiscriminatorUpgradeUI discriminatorUI;
        [SerializeField] private PlayerGymBuildingUI playerGymBuildingUI;

        [SerializeField] private GameObject restoreButton;
        [SerializeField] private GameObject noAdsButton;

        [Title("Black Screen")]
        [SerializeField] private Image blackScreenImage;

        private PoolSystem<PoolableObj> _moneyUIObjectPool;

        private Camera _cam;
        public HireWorkerBuildingUI HireWorkerUI => hireWorkerUI;
        public UpgradeCraneBuildingUI CraneUpgradeUI => craneUpgradeUI;

        public DiscriminatorUpgradeUI DiscriminatorUI => discriminatorUI;
        public PlayerGymBuildingUI PlayerGymBuildingUI => playerGymBuildingUI;

        protected override void Awake()
        {
            base.Awake();
            _cam = Camera.main;
            _moneyUIObjectPool = new PoolSystem<PoolableObj>(singleMoneyPiecePrefab, 5, gameplayCanvas.transform);
        }
        
        public void SpawnResourceUIPiece(Vector3 position, Action OnComplete)
        {
            Vector2 screenPos = _cam.WorldToScreenPoint(position);

            GameObject piece = _moneyUIObjectPool.PullGameObject();
            
            float radius = 25f;
            piece.transform.SetParent(gameplayCanvas.transform);
            piece.transform.position = screenPos + Random.insideUnitCircle * radius;
            piece.transform.localScale = Vector3.one;

            Sequence sequence = DOTween.Sequence();
            sequence.SetDelay(.2f);
            sequence.Append(piece.transform.DOMove(new Vector3(piece.transform.position.x - 30f, piece.transform.position.y - 30f, piece.transform.position.z), .5f));
            sequence.Append(piece.transform.DOMove(resourceUIPosition.position, .7f));
            sequence.OnComplete(() =>
            {
                OnComplete?.Invoke();
                piece.gameObject.SetActive(false);
            });
        }

        public void SetNOAdsStatus(bool status)
        {
            noAdsButton.SetActive(status);
        }
        
        public void SetRestoreAdsStatus(bool status)
        {
            restoreButton.SetActive(status);
        }

        public void ActivateBlackScreen(float activateDuration = .5f, float DeactiveDuration = .5f, float intervalBetweenFade = 1.5f, Action OnActivatedBlackScreen = null, Action OnDeactivatedBlackScreen = null, Action OnCompletedSequence = null)
        {
            Sequence seq = DOTween.Sequence();
            
            
            seq.Append(Scale(4f, activateDuration));
            seq.AppendCallback(() =>
            {
                Debug.Log("Appending?");
                OnActivatedBlackScreen?.Invoke();
            });
            seq.AppendInterval(intervalBetweenFade);
            seq.Append(Scale(0f, DeactiveDuration));
            seq.AppendCallback(() => OnDeactivatedBlackScreen?.Invoke());
            seq.OnComplete(() => { OnCompletedSequence?.Invoke(); });
        }

        private Tween FadeTo(float target, float duration)
        {
            return blackScreenImage.DOFade(target, duration).SetEase(Ease.Linear);
        }

        private Tween Scale(float multiplier, float duration)
        {
            return blackScreenImage.transform.DOScale(Vector3.one * multiplier, duration);
        }
    }
}