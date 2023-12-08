namespace _Project.Scripts
{
    public class AICollectDemolishPiecesState :  BaseAIState
    {
        public AICollectDemolishPiecesState(AIWorker worker) : base(worker)
        {
        }

        public override void Enter()
        {
            Worker.OnEnterCollectState();
        }

        public override void Tick()
        {
        }

        public override void Exit()
        {
        }
    }
}