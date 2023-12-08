using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using DG.Tweening;
using Lofelt.NiceVibrations;
using Safa_Packs.SM_V2;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class TinyHouseRequirementData
{
    public int minSpawnCount;
    public int maxSpawnCount;
    public int minRequirementAmount;
    public int maxRequirementAmount;
}

[Serializable]
public class TinyHouseRequirements
{
    [SerializeField] [ReadOnly] private int current;
    [SerializeField] [ReadOnly] public int total;
    public int rewardAmount = 10;

    [Title("Requirement Data")] public TinyHouseRequirementData[] requirementData;

    public Action<int, int> OnCurrentChanged;

    public int Current
    {
        get => current;
        set
        {
            current = value;
            OnCurrentChanged?.Invoke(current, total);
        }
    }

    public void SetTotal(int spawnCount)
    {
        foreach (var data in requirementData)
        {
            if (spawnCount > data.minSpawnCount && spawnCount <= data.maxSpawnCount)
            {
                total = Random.Range(data.minRequirementAmount, data.maxRequirementAmount);
                Debug.Log(spawnCount + "MinSpawnCount:" + data.minSpawnCount + "Max Spawn Count" + data.maxSpawnCount);
                break;
            }
            else
            {
                total = Random.Range(requirementData[^1].maxRequirementAmount,
                    requirementData[^1].maxRequirementAmount + 10);
            }
        }
    }
}

public class TinyHouseCar : MonoBehaviour
{
    [SerializeField] private ParticleSystem completeConfettiParticle;
    [SerializeField] private ParticleSystem dustBuildingParticle;
    [SerializeField] private Transform tinyHouseParent;
    [SerializeField] private TinyHouseRequirements requirements;
    [SerializeField] private AudioClip completedSound;
    [SerializeField] private GameObject[] tinyHouseModels;
    [Title("Transforms & Positions")] [SerializeField]
    private Transform demolishPieceArrivePoint;
    
    [Title("Cosmetics")] 
    [SerializeField] private MeshRenderer carFrontRenderer;
    [SerializeField] private Color[] carFrontColors;


    public Transform DemolishPieceArrivePoint => demolishPieceArrivePoint;

    private Vector3 _leaveAreaPosition;
    private Vector3 sellAreaPosition;

    public bool IsAvailable { get; private set; }

    public TinyHouseRequirements Requirements => requirements;

    public Action OnArrived;
    public Action OnArrivedExitArea;

    private CarGoLoadUpAreaState carGoLoadupState;
    private CarReadyToLoadState carReadyToLoadState;
    private CarLeaveAreaState carLeaveAreaState;
    private StateMachine _machine;


    private GameObject _selectedTinyHouse;
    private TinyHouseCarController _controller;

    public void InitCar(Vector3 leaveAreaPosition, Vector3 loadAreaPosition, TinyHouseCarController controller,
        int totalAmount)
    {
        _controller = controller;
        _leaveAreaPosition = leaveAreaPosition;
        sellAreaPosition = loadAreaPosition;
        requirements.SetTotal(totalAmount);

        InitCarStates();

        SetRandomTinyHouse();

        carFrontRenderer.material.color = carFrontColors[Random.Range(0, carFrontColors.Length)];
    }

    private void SetRandomTinyHouse()
    {
        foreach (GameObject model in tinyHouseModels) model.SetActive(false);
        _selectedTinyHouse = tinyHouseModels[Random.Range(0, tinyHouseModels.Length)];
    }

    private void InitCarStates()
    {
        _machine = new StateMachine();
        carGoLoadupState = new CarGoLoadUpAreaState(this);
        carReadyToLoadState = new CarReadyToLoadState(this);
        carLeaveAreaState = new CarLeaveAreaState(this);
        _machine.AddTransition(carReadyToLoadState, carLeaveAreaState, CompletedTinyHouse);
        ChangeState(carGoLoadupState);
    }

    private void Update()
    {
        _machine.Tick(false);
    }

    public void ChangeState(BaseState state)
    {
        _machine.SetState(state);
    }

    public void OnEnterCarGoLoadUpState()
    {
        MoveTo(sellAreaPosition, 2f, () => { ChangeState(carReadyToLoadState); });
    }

    public void OnEnterCarReadyToLoadState()
    {
        IsAvailable = true;
        OnArrived?.Invoke();
    }

    public void OnEnterCarLeaveAreaState()
    {
        TutorialManager.Instance.SetTutorialPanel(_controller.FirstJobCompletedTutorial, new[]
        {
            "Well done! You have completed your first job!",
            "You built wonderful house! You can be paid by building tiny houses!",
            "You can get your payment from your left, at 'Collect Money' indicator.",
            "Go get your money now!"
        });


        IsAvailable = false;
        _controller.OnTinyHouseCompleted?.Invoke(requirements.rewardAmount, _totalWorth);
        _totalWorth = 0;

        completeConfettiParticle.Play(true);
        SoundManager.Instance.PlaySound(completedSound);
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);

        Sequence seq = DOTween.Sequence();
        seq.SetDelay(1.5f);
        seq.Append(MoveTo(_leaveAreaPosition, 2f));
        seq.OnComplete(() => { OnArrivedExitArea?.Invoke(); });
    }

    // Checks if requirements fullfilled
    private bool CompletedTinyHouse()
    {
        return Requirements.Current >= Requirements.total;
    }

    // Load to the car
    private int _totalWorth;

    public void LoadUpCar(int worth)
    {
        if (CompletedTinyHouse()) return;
        if (!_selectedTinyHouse.activeInHierarchy) _selectedTinyHouse.SetActive(true);
        _totalWorth += worth;
        Requirements.Current++;
        float ratio = (float)Requirements.Current / (float)Requirements.total;
        var target = new Vector3(tinyHouseParent.localScale.x, ratio * 1f, tinyHouseParent.localScale.z);
        tinyHouseParent.DOScale(target, .1f);
        dustBuildingParticle.Play(true);
    }

    private Tween MoveTo(Vector3 position, float duration = .5f, Action OnComplete = null)
    {
        return transform.DOMove(position, duration).OnComplete(() => { OnComplete?.Invoke(); });
    }
}