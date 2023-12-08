using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts
{
    public class PlayerGymBuilding : BaseBuilding
    {
        private PlayerGymBuildingUI _ui;

        private PlayerWorkerData _data;
        public void InitBuilding(PlayerGymBuildingUI ui, PlayerWorkerData data)
        {
            _ui = ui;
            _ui.InitUI(this, data);
        }
        
        public override void Interact(InteractArgs args)
        {
            SetCooldown(() =>
            {
                base.Interact(args);
                _ui.Open();
                _ui.OnCloseUI += AllowInteractorToMove;
            });
        }
        
        protected override void AllowInteractorToMove()
        {
            base.AllowInteractorToMove();
            _ui.OnCloseUI -= AllowInteractorToMove;
        }
        
    }
}