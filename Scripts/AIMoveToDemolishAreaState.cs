namespace _Project.Scripts
{
    public class AIMoveToDemolishAreaState : BaseAIState
    {
        public AIMoveToDemolishAreaState(AIWorker worker) : base(worker)
        {
        }

        public override void Enter()
        {
            Worker.OnEnterMoveToDemolishState();
        }

        public override void Tick()
        {
            Worker.OnUpdateMoveToDemolishState();
        }

        public override void Exit()
        {
        }
    }
}