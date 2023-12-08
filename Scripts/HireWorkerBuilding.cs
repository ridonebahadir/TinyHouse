using System;
using _Project.Scripts;
using UnityEngine;

public class HireWorkerBuilding : BaseBuilding
{
    [SerializeField] private AIHireWorkerData demolisher;
    [SerializeField] private Transform workerSpawnPoint;
    
    private HireWorkerBuildingUI _ui;

    public AIHireWorkerData Demolisher => demolisher;

    private AIHireWorkerData[] workerDatas;
    
    public void InitBuilding(HireWorkerBuildingUI ui, AIHireWorkerData[] availableWorkersData)
    {
        workerDatas = availableWorkersData;
        _ui = ui;
        _ui.InitUI(this, availableWorkersData);

        foreach (var workerData in availableWorkersData) if (workerData.HasBoughtHireWorker) SpawnWorker(workerData);
    }
    
    public override void Interact(InteractArgs args)
    {
        SetCooldown(() =>
        {
            base.Interact(args);
            _ui.InitUI(this, workerDatas);
            _ui.Open();
            _ui.OnCloseUI += AllowInteractorToMove;
        });
    }
    
    public void SpawnWorker(AIHireWorkerData data)
    {
        AIWorker worker = data.SpawnWorker(workerSpawnPoint.position, AssignedArea);
        AssignedArea.AddWorker(worker);
    }
    
    public AIWorker SpawnDemolisher(AreaController area)
    {
        AIWorker worker = demolisher.SpawnWorker(workerSpawnPoint.position, area);
        //AssignedArea.AddWorker(worker);
        return worker;
    }
    
    protected override void AllowInteractorToMove()
    {
        base.AllowInteractorToMove();
        _ui.OnCloseUI -= AllowInteractorToMove;
    }
}
