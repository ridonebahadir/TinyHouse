using Safa_Packs.SM_V2;

namespace _Project.Scripts
{
    public abstract class BaseAIState : BaseState
    {
        protected BaseAIState(AIWorker worker)
        {
            Worker = worker;
        }

        protected AIWorker Worker { get; }
    }
}