namespace _Project.Scripts
{
    public class AIMoveToSellDiscriminator : BaseAIState
    {
        public AIMoveToSellDiscriminator(AIWorker worker) : base(worker)
        {
        }

        public override void Enter()
        {
            Worker.OnEnterMoveToSellDiscriminatorStartState();
        }

        public override void Tick()
        {
            Worker.OnUpdateMoveToSellDiscriminatorStartState();
        }

        public override void Exit()
        {
        }
    }
}