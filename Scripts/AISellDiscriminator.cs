namespace _Project.Scripts
{
    public class AISellDiscriminator : BaseAIState
    {
        public AISellDiscriminator(AIWorker worker) : base(worker)
        {
        }

        public override void Enter()
        {
            Worker.OnEnterSellDiscriminatorStartState();
        }

        public override void Tick()
        {
        }

        public override void Exit()
        {
        }
    }
}