using System;
using _Project.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "CraneStats", order = 1)]
public class CraneStats : ScriptableObject
{
    public string id;
    public AreaController AssignedArea { get; set; }
    
    private int increasePiece;
    public int increaseInterval;
    [ReadOnly] public int CollectedPieces;

    [Title("Arm Length Upgrade")] 
    [SerializeField] private float armLength;
    public Sprite armUpgradeSprite;
    public string armUpgradeDescription;

    [Title("Vacuum Upgrade")]
    [SerializeField] private float vacuumScale = 1f;
    public Sprite vacuumScaleUpgradeSprite;
    public string vacuumScaleDescription;
    public int maxVacuumLevel = 7;


    [Title("Ball Upgrade")]
    [SerializeField] private float ballScale = 1f;
    public Sprite ballScaleUpgradeSprite;
    public string ballScaleDescription;
    public int maxBallLevel = 7;
    

    public Action OnArmLengthChanged;
    public float CurrentArmLength
    {
        get => ES3.Load($"{name}_armLength", armLength);
        set
        {
            ES3.Save($"{name}_armLength", value);
            OnArmLengthChanged?.Invoke();
        }
    }
    
    public Action OnVacuumScaleChanged;
    public Action OnBallScaleChanged;
    public float CurrentVacuumScale
    {
        get => ES3.Load($"{name}_vacuumScale", 0f);
        set
        {
            ES3.Save($"{name}_vacuumScale", value);
            OnVacuumScaleChanged?.Invoke();
        }
    }
    public float CurrentBallScale
    {
        get => ES3.Load($"{name}_ballScale", 0f);
        set
        {
            ES3.Save($"{name}_ballScale", value);
            OnBallScaleChanged?.Invoke();
        }
    }
    
    public int CurrentVacuumLevel
    {
        get => ES3.Load($"{name}_vacuumLevel", 1);
        set => ES3.Save($"{name}_vacuumLevel", value);
    }
    
    public int CurrentArmLengthLevel
    {
        get => ES3.Load($"{name}ArmLengthLevel", 1);
        set => ES3.Save($"{name}ArmLengthLevel", value);
    }
    
    public int CurrentBallLevel
    {
        get => ES3.Load($"{name}BallLevel", 1);
        set => ES3.Save($"{name}BallLevel", value);
    }

    
    public float GetVacuumUpgradeCost(bool isOnBoardLevel)
    {
        var lastLevel = AssignedArea.AreaLevel;

        if (isOnBoardLevel)
        {
            return Formulas.GetOnboardVacuumCosts(CurrentVacuumLevel - 1, lastLevel);
        }
        return Formulas.GetRegularVacuumCosts(CurrentVacuumLevel - 1, lastLevel);
    }
    
    public float GetBallScaleUpgradeCost(bool isOnBoardLevel)
    {
        var lastLevel = AssignedArea.AreaLevel;
        if (isOnBoardLevel)
        {
            return Formulas.GetOnboardBallScaleCosts(CurrentBallLevel - 1, lastLevel);
        }
        return Formulas.GetRegularBallScaleCosts(CurrentBallLevel - 1, lastLevel);
    }

    public float GetArmLengthCost(bool isOnBoardLevel)
    {
        var lastLevel = AssignedArea.AreaLevel;
        if (isOnBoardLevel)
        {
            return Formulas.GetOnboardCraneLengthCost(CurrentArmLengthLevel);
        }
        return Formulas.GetRegularCraneLengthCost(CurrentArmLengthLevel, lastLevel);
        
    }

    public void ResetCollectedPieces()
    {
        increasePiece = 0;
        CollectedPieces = 0;
    }

   
    public void Increase()
    {
        increasePiece++;
        if (increasePiece%increaseInterval==0)
        {
            CollectedPieces++;
        }
    }
}