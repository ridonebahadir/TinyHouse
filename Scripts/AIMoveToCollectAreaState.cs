namespace _Project.Scripts
{
    public class AIMoveToCollectAreaState : BaseAIState
    {
        public AIMoveToCollectAreaState(AIWorker worker) : base(worker)
        {
        }

        public override void Enter()
        {
            Worker.OnEnterMoveToCollectState();
        }

        public override void Tick()
        {
            Worker.OnUpdateMoveToCollectState();
        }

        public override void Exit()
        {
        }
    }
}