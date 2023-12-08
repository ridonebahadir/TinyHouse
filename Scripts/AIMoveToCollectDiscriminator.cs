namespace _Project.Scripts
{
    public class AIMoveToCollectDiscriminator : BaseAIState
    {
        public AIMoveToCollectDiscriminator(AIWorker worker) : base(worker)
        {
        }

        public override void Enter()
        {
            Worker.OnEnterMoveToCollectDiscriminatorEnd();
        }

        public override void Tick()
        {
            Worker.OnUpdateMoveToCollectDiscriminatorEnd();
        }

        public override void Exit()
        {
        }
    }
}