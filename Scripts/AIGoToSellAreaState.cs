namespace _Project.Scripts
{
    public class AIGoToSellAreaState : BaseAIState
    {
        public AIGoToSellAreaState(AIWorker worker) : base(worker)
        {
        }

        public override void Enter()
        {
            Worker.OnEnterMoveToSellState();
        }

        public override void Tick()
        {
            Worker.OnUpdateMoveToSellState();
        }

        public override void Exit()
        {
        }
    }
}