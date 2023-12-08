using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    public class FirstAreaOnboarding : MonoBehaviour
    {
        [Title("Tutorials")]
        [SerializeField] private TutorialData unlockedHireWorkerIndicatorTutorial;
        [SerializeField] private TutorialData unlockedPlayerGymIndicatorTutorial;
        [SerializeField] private TutorialData unlockedCraneIndicatorTutorial;
        [SerializeField] private TutorialData collectAndSellTutorial;
        
        
        [Title("Required Costs for Tutorial Execution")]
        [SerializeField] private int level1CostUnlockCraneIndicator = 1000;
        [SerializeField] private int level1UnlockHireWorkerIndicatorCost = 2000;
        [SerializeField] private int level1PlayerGymCost = 6000;
        
        [Title("Resource")]
        [SerializeField] private ResourceSo resource;
       
        private AreaController _controlller;
        private TutorialManager manager;

        private void Awake()
        {
            _controlller = GetComponent<AreaController>();
        }

        private void Start()
        {
            manager = TutorialManager.Instance;
        }

        private void OnEnable()
        {
            resource.OnValueChanged += UnlockCraneUpgradeIndicator;
            resource.OnValueChanged += UnlockHireWorkerIndicator;
            resource.OnValueChanged += UnlockPlayerGymIndicator;
            GameEventPool.OnCraneAreaOnBoardCompleted += TrySetFirstCollectAndSellTutorial;
        }

        private void TrySetFirstCollectAndSellTutorial()
        {
            GameEventPool.OnCraneAreaOnBoardCompleted -= TrySetFirstCollectAndSellTutorial;

            TutorialManager.Instance.SetTutorialPanel(collectAndSellTutorial, new[]
            {
                "Great! Now collect your pieces..",
                "And sell them to the car!",
                "You can find 'Collect' and 'Sell' Indicators on area.",
                "Good luck"
            }, () =>
            {
                Vector3 collectAreaPos = AreaManager.Instance.DefaultArea.PieceCollectArea.InteractPosition;
                Vector3 sellAreaPos = AreaManager.Instance.DefaultArea.PieceSellArea.InteractPosition;

                StartCoroutine(IE_SetTutorialSequence());

                IEnumerator IE_SetTutorialSequence()
                {
                    TutorialLine tutLine = TutorialManager.Instance.SetTutorialLine(AreaManager.Instance.PlayerWorker.transform, collectAreaPos);
                    
                    yield return new WaitUntil(() => !tutLine.gameObject.activeInHierarchy);
                    
                    TutorialManager.Instance.SetTutorialLine(AreaManager.Instance.PlayerWorker.transform, sellAreaPos);
                }
            });
        }

        private void UnlockCraneUpgradeIndicator(int cost)
        {
            var indicator = _controlller.GetUpgradeCraneIndicator();
            if (indicator.UnlockedIndicator) return;

            if (cost < level1CostUnlockCraneIndicator) return;

            // Activate indicator on arrived resource
            _controlller.ActivateUpgradeCraneIndicator();
            manager.SetCameraTO(indicator.transform, 2f, 2f, () =>
            {
                manager.SetTutorialPanel(unlockedCraneIndicatorTutorial, new[]
                {
                    "Crane Upgrader will help us to empower our crane!",
                    "Lets go and build it.",
                }, () =>
                {
                    Vector3 unlockCraneIndicatorPos = indicator.transform.position;
                    TutorialManager.Instance.SetTutorialLine(AreaManager.Instance.PlayerWorker.transform, unlockCraneIndicatorPos);

                });
            });

            resource.OnValueChanged -= UnlockCraneUpgradeIndicator;
        }



        private void UnlockHireWorkerIndicator(int cost)
        {
            var indicator = _controlller.GetHireWorkerIndicator();
            if (indicator.UnlockedIndicator) return;

            // Activate indicator on arrived resource
            if (cost < level1UnlockHireWorkerIndicatorCost) return;

            _controlller.ActivateHireWorkerIndicator();

            manager.SetCameraTO(indicator.transform, 2f, 2f, () =>
            {
                manager.SetTutorialPanel(unlockedHireWorkerIndicatorTutorial, new[]
                {
                    "You can now hire workers to help you!",
                    "Lets hire a new worker!",
                }, () =>
                {
                    Vector3 hireWorker = indicator.transform.position;
                    TutorialManager.Instance.SetTutorialLine(AreaManager.Instance.PlayerWorker.transform, hireWorker);
                });
            });

            resource.OnValueChanged -= UnlockHireWorkerIndicator;
        }

        private void UnlockPlayerGymIndicator(int cost)
        {
            var indicator = _controlller.GetPlayerGymIndicator();
            if (indicator.UnlockedIndicator) return;
            if (cost < level1PlayerGymCost) return;

            _controlller.ActivatePlayerGymIndicator();
            manager.SetCameraTO(indicator.transform, 2f, 2f, () =>
            {
                manager.SetTutorialPanel(unlockedPlayerGymIndicatorTutorial, new[]
                {
                    "You can now upgrade your player! ",
                    "Lets go there and build the gym!",
                }, () =>
                {
                    Vector3 playerGym = indicator.transform.position;
                    TutorialManager.Instance.SetTutorialLine(AreaManager.Instance.PlayerWorker.transform, playerGym);
                });
            });
            resource.OnValueChanged -= UnlockPlayerGymIndicator;
        }
    }
}