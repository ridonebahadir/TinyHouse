namespace _Project.Scripts
{
    public class AIDemolishState : BaseAIState
    {
        public AIDemolishState(AIWorker worker) : base(worker)
        {
        }

        public override void Enter()
        {
            Worker.OnEnterDemolishState();
        }

        public override void Tick()
        {
        }

        public override void Exit()
        {
        }
    }
}