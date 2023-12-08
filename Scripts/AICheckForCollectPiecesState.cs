namespace _Project.Scripts
{
    public class AICheckForCollectPiecesState : BaseAIState
    {
        public AICheckForCollectPiecesState(AIWorker worker) : base(worker)
        {
        }

        public override void Enter()
        {
            Worker.OnEnterCheckForCollectState();
        }

        public override void Tick()
        {
            Worker.OnUpdateCheckForCollectState();
        }

        public override void Exit()
        {
            Worker.OnExitCheckForCollectState();
        }
    }
}