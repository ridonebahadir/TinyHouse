namespace _Project.Scripts
{
    public class UpgradeCraneBuilding : BaseBuilding
    {
        private UpgradeCraneBuildingUI _ui;
        public void InitUI(UpgradeCraneBuildingUI ui)
        {
            _ui = ui;
            _ui.InitUI(this);
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