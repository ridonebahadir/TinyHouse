using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class ArmLengthUpgradeContainer : BaseCraneUpgradeContainer
    {
        protected override Sprite GetUpgradeSprite()
        {
            return _stats.armUpgradeSprite;
        }
        protected override int GetUpgradeCost()
        {
            return (int) _stats.GetArmLengthCost(_stats.AssignedArea.OnBoardingLevel);
        }

        protected override int GetLevel()
        {
            return _stats.CurrentArmLengthLevel;
        }

        protected override int GetMaxLevel() => Int32.MaxValue;

        protected override string GetDescriptionText()
        {
            return _stats.armUpgradeDescription;
        }

        protected override void UpgradeStats()
        {
            _stats.CurrentArmLength += 7.5f;
            _stats.CurrentArmLengthLevel++;
        }
    }
}