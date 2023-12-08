using Unity.Services.RemoteConfig;

namespace _Project.Scripts
{
    public static class RemoteConfigValues
    {
        public static int ExpenseMultiplier = RemoteConfigService.Instance.appConfig.GetInt(nameof(ExpenseMultiplier), 20);
        public static int GainMultiplier = RemoteConfigService.Instance.appConfig.GetInt(nameof(GainMultiplier), 18);
        public static int IncomeExpenseBase = RemoteConfigService.Instance.appConfig.GetInt(nameof(IncomeExpenseBase), 25000);
        public static int LevelBalanceGainsBase = RemoteConfigService.Instance.appConfig.GetInt(nameof(LevelBalanceGainsBase), 200);
        public static int OnboardingLevelCraneUpgradeExpenseBase = RemoteConfigService.Instance.appConfig.GetInt(nameof(OnboardingLevelCraneUpgradeExpenseBase),50);
        public static int OnboardingLevelHireWorkerExpenseBase = RemoteConfigService.Instance.appConfig.GetInt(nameof(OnboardingLevelHireWorkerExpenseBase),500);
        public static int PlayerWorkerUpgradeBase = RemoteConfigService.Instance.appConfig.GetInt(nameof(PlayerWorkerUpgradeBase), 200);
        public static int RegularLevelCraneUpgradeExpenseBase = RemoteConfigService.Instance.appConfig.GetInt(nameof(RegularLevelCraneUpgradeExpenseBase), 1000);
        public static int RegularLevelHireWorkerExpenseBase = RemoteConfigService.Instance.appConfig.GetInt(nameof(RegularLevelHireWorkerExpenseBase), 25000);
        
        
        
    }
}