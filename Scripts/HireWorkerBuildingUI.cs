using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class HireWorkerBuildingUI : BaseBuildingUI
    {
        [SerializeField] private HireWorkerContainerUI containerUI;
        [SerializeField] private Transform verticalGroupParent;
        private List<HireWorkerContainerUI> _activeContainers = new List<HireWorkerContainerUI>();

        [SerializeField] private TutorialData hiredWorkerTutorial;
        
        public HireWorkerBuilding Building { get; private set; }

        public TutorialData HiredWorkerTutorial => hiredWorkerTutorial;

        private AIHireWorkerData[] workersData;

        public void InitUI(HireWorkerBuilding building, AIHireWorkerData[] aiHireWorkerDatas)
        {
            workersData = aiHireWorkerDatas;
            Building = building;
            
            foreach (Transform child in verticalGroupParent) Destroy(child.gameObject);

            foreach (var workerData in aiHireWorkerDatas)
            {
                var container = Instantiate(containerUI, verticalGroupParent);
                container.InitData(workerData,this);
                _activeContainers.Add(container);
            }
        }
    }
}