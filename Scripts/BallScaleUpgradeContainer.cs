using UnityEngine;

namespace _Project.Scripts
{
    public class BallScaleUpgradeContainer : BaseCraneUpgradeContainer
    {
        protected override Sprite GetUpgradeSprite()
        {
            return _stats.ballScaleUpgradeSprite;
        }

        protected override int GetUpgradeCost()
        {
            return (int)_stats.GetBallScaleUpgradeCost(_stats.AssignedArea.OnBoardingLevel);
        }

        protected override int GetLevel()
        {
            return _stats.CurrentBallLevel;
        }

        protected override int GetMaxLevel()
        {
            return _stats.maxBallLevel;
        }

        protected override string GetDescriptionText()
        {
            return _stats.ballScaleDescription;
        }

        protected override void UpgradeStats()
        {
            _stats.CurrentBallScale = Mathf.Min(_stats.CurrentBallScale + .3f, 2f);
            _stats.CurrentBallLevel++;
        }
    }
}