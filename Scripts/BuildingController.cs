using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BuildingController : MonoBehaviour, IAreaElement
{
    [Title("Hire Worker Building Data")] [SerializeField]
    private AIHireWorkerData[] availableWorkersData;

    [SerializeField] private HireWorkerBuilding hireWorkerBuildingPrefab;

    [Title("Player Gym Data")] [SerializeField]
    private PlayerWorkerData playerWorkerData;

    [SerializeField] private PlayerGymBuilding playerGymBuildingPrefab;

    [Title("Crane Upgrade Building Settings")] [SerializeField]
    private UpgradeCraneBuilding upradeCraneBuildingPrefab;

    [Title("Discriminator Building")] [SerializeField]
    private Discriminator discriminatorPrefab;

    [SerializeField] private IndicatorDiscriminatorStart discriminatorStartPrefab;
    [SerializeField] private IndicatorDiscriminatorEnd discriminatorEndPrefab;

    public HireWorkerBuilding CurrentHireBuildingInArea { get; private set; }
    public UpgradeCraneBuilding CurrentCraneBuildingInArea { get; private set; }

    public AIHireWorkerData GetRandomWorkerData()
    {
        return availableWorkersData[Random.Range(0, availableWorkersData.Length)];
    }

    private TBuilding SpawnBuilding<TBuilding>(TBuilding prefab, Vector3 position) where TBuilding : BaseBuilding
    {
        var building = Instantiate(prefab, position, prefab.transform.rotation);
        building.transform.SetParent(transform);
        return building;
    }

    public void SpawnHireWorkerBuilding(Vector3 position)
    {
        // Initialize building
        CurrentHireBuildingInArea = SpawnBuilding(HireWorkerBuildingPrefab, position);

        // Initialize its area.
        CurrentHireBuildingInArea.InitArea(AssignedArea);

        // Introduce UI to the hire building
        CurrentHireBuildingInArea.InitBuilding(UIManager.Instance.HireWorkerUI, availableWorkersData);
    }

    public void SpawnPlayerGym(Vector3 position)
    {
        PlayerGymBuilding gymBuilding = SpawnBuilding(playerGymBuildingPrefab, position);

        // Initialize area
        gymBuilding.InitArea(AssignedArea);


        gymBuilding.InitBuilding(UIManager.Instance.PlayerGymBuildingUI, playerWorkerData);
    }

    public IndicatorDiscriminatorStart SpawnDiscriminatorStart(Vector3 position, PieceType[] types)
    {
        var start = Instantiate(discriminatorStartPrefab, position, Quaternion.identity);
        start.RequiredTypes = types;
        start.transform.position = position;
        return start;
    }
    public IndicatorDiscriminatorEnd SpawnDiscriminatorEnd(Vector3 position)
    {
        var end = Instantiate(discriminatorEndPrefab, position, Quaternion.identity);
        end.transform.position = position;
        return end;
    }
    public Discriminator SpawnDiscriminator(PieceData[] data, PieceType type, Vector3 discriminatorPosition, int idx, string saveID, IndicatorDiscriminatorStart start, IndicatorDiscriminatorEnd end)
    {
        // Initialize building
        var spawnedBuilding = SpawnBuilding(discriminatorPrefab, discriminatorPosition);
        spawnedBuilding.transform.position = discriminatorPosition;
        spawnedBuilding.gameObject.name = $"Dicriminator_{AssignedArea.AreaLevel}_{AssignedArea.AreaID}_{saveID}";
        spawnedBuilding.InitBuilding(UIManager.Instance.DiscriminatorUI, idx);
        spawnedBuilding.InitializeDiscriminator(data, type, AssignedArea, start, end);
        // Introduce Building to the UI
        // Initialize its area.
        
        foreach (var worker in AssignedArea.ActiveWorkers) worker.CreateDiscriminatorStates();

        return spawnedBuilding;
        // Initialize UI workers
    }


    public void SpawnUpgradeCraneBuilding(Vector3 position)
    {
        var spawnedBuilding = SpawnBuilding(upradeCraneBuildingPrefab, position);
        spawnedBuilding.InitArea(AssignedArea);
        spawnedBuilding.InitUI(UIManager.Instance.CraneUpgradeUI);
        CurrentCraneBuildingInArea = spawnedBuilding;
    }

    public string AreaID { get; set; }
    public AreaController AssignedArea { get; set; }

    public HireWorkerBuilding HireWorkerBuildingPrefab => hireWorkerBuildingPrefab;

    public void InitArea(AreaController area)
    {
        AssignedArea = area;
        AreaID = area.AreaID;
    }
}