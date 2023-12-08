namespace _Project.Scripts
{
    public class AICollectDiscriminator : BaseAIState
    {
        public AICollectDiscriminator(AIWorker worker) : base(worker)
        {
        }

        public override void Enter()
        {
            Worker.OnEnterCollectDiscriminatorEnd();
        }

        public override void Tick()
        {
        }

        public override void Exit()
        {
        }
    }
}