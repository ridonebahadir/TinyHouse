using System;
using System.Collections.Generic;
using _Project.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

public static class Formulas
{
    //Expenses kat sayi
    private static readonly int _expenseMultiplier = RemoteConfigValues.ExpenseMultiplier; 
    private static readonly int _gainMultiplier = RemoteConfigValues.GainMultiplier; //Gains kat sayi
    private static readonly int _incomeExpenseBase = RemoteConfigValues.IncomeExpenseBase; //base
    private static readonly int levelBalanceGainsBase = RemoteConfigValues.LevelBalanceGainsBase; //base
    private static readonly int _onboardingLevelCraneUpgradeExpenseBase = RemoteConfigValues.OnboardingLevelCraneUpgradeExpenseBase;
    private static readonly int _onboardingLevelHireWorkerExpenseBase = RemoteConfigValues.OnboardingLevelHireWorkerExpenseBase;
    private static readonly int _playerUpgradeBase = RemoteConfigValues.PlayerWorkerUpgradeBase;
    private static readonly int _regularLevelCraneUpgradeExpenseBase = RemoteConfigValues.RegularLevelCraneUpgradeExpenseBase;
    private static readonly int _regularLevelHireWorkerExpenseBase = RemoteConfigValues.RegularLevelHireWorkerExpenseBase;
    
    
    private static readonly List<int> _incomeGains = new List<int> { 15, 29, 45, 40, 85, 180 }; //List
    private static readonly List<int> _discriminatorUpgradeCosts = new List<int> { 14, 35,48,96 }; //List
    private static readonly List<int> _vacuumUpgradeCosts = new List<int>() { 5, 10, 25, 50, 100, 200, 300, 400, 500 };
    private static readonly List<int> _ballScaleUpgradeCosts = new List<int>() { 5, 10, 15, 25, 50, 75, 100, 125, 150 };


    public static float GetGainFormula(int idx, int lastLevel)
    {
        //base*l[i]*n^(map-2)
        var gains = (levelBalanceGainsBase * _incomeGains[idx]) * Mathf.Pow(_gainMultiplier, lastLevel - 2);

        Debug.Log(
            $"Gain Base{levelBalanceGainsBase}, Income IDX: {_incomeGains[idx]}, ÜS: ${Mathf.Pow(_gainMultiplier, lastLevel - 2)}, last level: {lastLevel}");
        Debug.Log("Gains " + gains);
        return gains;
    }

    private static float GetExpenseFormula(List<int> costsByLevel, float baseValue, int idx, int currentMapLevel)
    {
        //base*l[i]*m^(map-2)
        var expenses = (baseValue * costsByLevel[idx]) * Mathf.Pow(_expenseMultiplier, currentMapLevel - 2);
        Debug.Log("Expenses " + expenses);
        return expenses;
    }

    public static float GetOnboardVacuumCosts(int idx, int currentMapLevel)
    {
        if (idx >= _vacuumUpgradeCosts.Count) return _onboardingLevelCraneUpgradeExpenseBase * _vacuumUpgradeCosts[^1];
        return _onboardingLevelCraneUpgradeExpenseBase * _vacuumUpgradeCosts[idx];
    }

    public static float GetRegularVacuumCosts(int idx, int currentMapLevel)
    {
        if (idx >= _vacuumUpgradeCosts.Count) return GetExpenseFormula(_vacuumUpgradeCosts, _regularLevelCraneUpgradeExpenseBase, _vacuumUpgradeCosts[^1], currentMapLevel);
        return GetExpenseFormula(_vacuumUpgradeCosts, _regularLevelCraneUpgradeExpenseBase, idx, currentMapLevel);
    }
    
    public static float GetOnboardBallScaleCosts(int idx, int currentMapLevel)
    {
        if (idx >= _ballScaleUpgradeCosts.Count) return _onboardingLevelCraneUpgradeExpenseBase * _ballScaleUpgradeCosts[^1];
        return _onboardingLevelCraneUpgradeExpenseBase * _ballScaleUpgradeCosts[idx];
    }
    
    public static float GetRegularBallScaleCosts(int idx, int currentMapLevel)
    {
        if (idx >= _ballScaleUpgradeCosts.Count) return GetExpenseFormula(_ballScaleUpgradeCosts, _regularLevelCraneUpgradeExpenseBase, _ballScaleUpgradeCosts[^1], currentMapLevel);
        return GetExpenseFormula(_ballScaleUpgradeCosts, _regularLevelCraneUpgradeExpenseBase, idx, currentMapLevel);
    }

    public static float GetDiscriminatorUpgradeCosts(int idx, int currentMapLevel)
    {
        return GetExpenseFormula(_discriminatorUpgradeCosts, _incomeExpenseBase, idx, currentMapLevel);
    }

    public static float GetOnboardCraneLengthCost(int upgradeLevel)
    {
        if (upgradeLevel == 1) return _onboardingLevelCraneUpgradeExpenseBase;
        return _onboardingLevelCraneUpgradeExpenseBase * 2.5f * (upgradeLevel - 1);
    }
    
    public static float GetRegularCraneLengthCost(int upgradeLevel, int currentMapLevel)
    {
        if (upgradeLevel == 1) return  _regularLevelCraneUpgradeExpenseBase * Mathf.Pow(_expenseMultiplier, currentMapLevel - 2);
        return _regularLevelCraneUpgradeExpenseBase * 2.5f * (upgradeLevel - 1) ;
    }

    public static float GetPlayerUpgradeCost(int upgradeLevel)
    {
        //base*(20^((n-1)//5))*(((n-1)%5)*2+1)
        var price = _playerUpgradeBase * ((Mathf.Pow(20, ((int)((upgradeLevel - 1) / 5)))) * (((upgradeLevel - 1) % 5) * 2 + 1));
        Debug.Log("Price " + price);
        return price;
    }

    public static int GetWorkerCost(int upgradeLevel, bool onBoard, int currentMapLevel)
    {
        if (onBoard)
            return _onboardingLevelHireWorkerExpenseBase * upgradeLevel;
        return Mathf.RoundToInt(_regularLevelHireWorkerExpenseBase * upgradeLevel * Mathf.Pow(_expenseMultiplier, currentMapLevel - 2));
    }
}