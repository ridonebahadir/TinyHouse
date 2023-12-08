using UnityEngine;

namespace _Project.Scripts
{
    public class PlayerGymBuildingUI : BaseBuildingUI
    {
        [SerializeField] private PlayerGymContainerUI containerUI;
        [SerializeField] private Transform verticalGroupParent;
        public PlayerGymBuilding Building { get; private set; }

        private PlayerGymContainerUI _activeContainer;

        public void InitUI(PlayerGymBuilding building, PlayerWorkerData playerWorkerData)
        {
            Building = building;
            foreach (Transform child in verticalGroupParent) Destroy(child.gameObject);
            _activeContainer = Instantiate(containerUI, verticalGroupParent);
            _activeContainer.InitData(playerWorkerData, this);
        }

        public override void Open()
        {
            base.Open();
            _activeContainer.InitUI();
        }
    }
}