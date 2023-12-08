using UnityEngine;

namespace _Project.Scripts
{
    public class VacuumLengthUpgradeContainer : BaseCraneUpgradeContainer
    {
        protected override Sprite GetUpgradeSprite()
        {
            return _stats.vacuumScaleUpgradeSprite;
        }

        protected override int GetUpgradeCost()
        {
            return (int) _stats.GetVacuumUpgradeCost(_stats.AssignedArea.OnBoardingLevel);
        }

        protected override int GetLevel()
        {
            return _stats.CurrentVacuumLevel;
        }

        protected override int GetMaxLevel()
        {
            return _stats.maxVacuumLevel;
        }

        protected override string GetDescriptionText()
        {
            return _stats.vacuumScaleDescription;
        }

        protected override void UpgradeStats()
        {
            _stats.CurrentVacuumScale = Mathf.Min(_stats.CurrentVacuumScale + .3f, 2f);
            _stats.CurrentVacuumLevel++;
        }
    }
}