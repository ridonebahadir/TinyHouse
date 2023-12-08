using Safa_Packs.SM_V2;

namespace _Project.Scripts
{
    public class AISellPiecesState : BaseAIState
    {
        public AISellPiecesState(AIWorker worker) : base(worker)
        {
        }

        public override void Enter()
        {
            Worker.OnEnterSellState();
        }

        public override void Tick()
        {
            Worker.OnUpdateSellState();
        }

        public override void Exit()
        {
            Worker.OnExitSellState();
        }
    }
}