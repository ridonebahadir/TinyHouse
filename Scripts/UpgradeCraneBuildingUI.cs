using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts
{
    public class UpgradeCraneBuildingUI : BaseBuildingUI
    {
        [SerializeField] private Transform containerParent;
        [SerializeField] private BaseCraneUpgradeContainer[] containers;
        [SerializeField] private TutorialData cranePanelClosedTutorial;
        private List<BaseCraneUpgradeContainer> _activeContainers = new List<BaseCraneUpgradeContainer>();

        public UpgradeCraneBuilding Building;

        protected override void Start()
        {
            base.Start();

            foreach (BaseCraneUpgradeContainer container in containers)
            {
                BaseCraneUpgradeContainer obj = Instantiate(container, containerParent);
                _activeContainers.Add(obj);
            }
        }

        public void InitUI(UpgradeCraneBuilding building)
        {
            Building = building;
        }

        public override void Open()
        {
            base.Open();

            foreach (BaseCraneUpgradeContainer container in _activeContainers)
            {
                container.InitContainer(this);
                container.UpdateUI();
                container.ButtonInteractable(cranePanelClosedTutorial.ShownTutorial);
            }

            //Tutorial Crane Upgrade
            if (cranePanelClosedTutorial.ShownTutorial) return;
            TutorialManager.Instance.SetFingerObj(true);
            _activeContainers[0].ButtonInteractable(true);
            ActivateCloseButton(false);
        }

        public void ActivateCloseButton(bool status)
        {
            closeButton.gameObject.SetActive(status);
        }

        public override void Close()
        {
            base.Close();

            TutorialManager.Instance.SetTutorialPanel(cranePanelClosedTutorial,
                new[]
                {
                    "To improve your job, you need to demolish buildings..",
                    "and improve your base by upgrading your crane..",
                    "by hiring workers..",
                    "and expanding your area.",
                    "Now go to 'Demolish' indicator and earn some money!"
                }, () =>
                {
                    Vector3 demolishArea = AreaManager.Instance.DefaultArea.PieceDemolishArea.InteractPosition;
                    TutorialManager.Instance.SetTutorialLine(AreaManager.Instance.PlayerWorker.transform, demolishArea);
                });
        }
    }
}