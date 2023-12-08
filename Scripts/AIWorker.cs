using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts
{
    public class AIWorker : BaseWorker
    {
        [SerializeField] private GameObject model;
        [SerializeField] private ChatBubble bubble;
        
        protected StateMachine Machine;
        public AIMoveToCollectAreaState MoveToCollectState { get; private set; }
        public AICollectDemolishPiecesState CollectPiecesState { get; private set; }
        public AIGoToSellAreaState MoveToSellState { get; private set; }
        public AISellPiecesState SellPiecesState { get; private set; }
        public AICheckForCollectPiecesState CheckCollectPiecesState { get; private set; }

        public IInteractable CurrentInteractable { get; private set; }
        public IInteractable CollectArea { get; private set; }
        public IInteractable SellArea { get; private set; }

        private NavMeshAgent _agent;

        private AIHireWorkerData _data;

        private AreaController _currentArea;

        [SerializeField] private string debugState;

        protected override void Awake()
        {
            base.Awake();
            _agent = GetComponent<NavMeshAgent>();
            Machine = new StateMachine();

            // Init interactions

            // Init AI States
            // Move to the Collect Area
            // Collect all the pieces if available; 
            // Go to sell area
            // Sell all the pieces
            // Loop
            MoveToCollectState = new AIMoveToCollectAreaState(this);
            CollectPiecesState = new AICollectDemolishPiecesState(this);
            MoveToSellState = new AIGoToSellAreaState(this);
            SellPiecesState = new AISellPiecesState(this);
            CheckCollectPiecesState = new AICheckForCollectPiecesState(this);

            Machine.AddTransition(MoveToCollectState, CheckCollectPiecesState, CanCheckForCollection);
            Machine.AddTransition(CheckCollectPiecesState, CollectPiecesState, HasStackOnCollectArea);
            Machine.AddTransition(CollectPiecesState, MoveToSellState, CanGoToSellArea);
            Machine.AddTransition(MoveToSellState, SellPiecesState, IsInSellIndicatorRange);
            Machine.AddTransition(SellPiecesState, MoveToCollectState, () => HasNoStack() && HasStackOnCollectArea());
            Machine.SetState(MoveToCollectState);
            
        }

        public void InitAI(AIHireWorkerData data, AreaController currentArea)
        {
            InitializeInteractions(currentArea);
            _data = data;
            ApplyStats();
            if (_currentArea.PieceDiscriminatorStartArea != null) CreateDiscriminatorStates();
        }

        public override void Update()
        {
#if UNITY_EDITOR
            debugState = Machine.CurrentState.GetType().ToString();
#endif
            base.Update();
            Machine.Tick();
        }

        private bool CanCheckForCollection()
        {
            return IsInDistanceWith(CollectArea.InteractPosition, interactionDistance);
        }

        private bool HasStackOnCollectArea()
        {
            return CollectArea.CanInteract;
        }

        private bool CanGoToSellArea()
        {
            return !CollectArea.CanInteract || !CanAddToStack();
        }
        
        private bool CollectedEnoughFromDiscriminatorCollectArea()
        {
            return !_currentArea.PieceDiscriminatorEndArea.CanInteract || !CanAddToStack();
        }
        
        private bool CollectedEnoughFromCollectArea()
        {
            return !CollectArea.CanInteract || !CanAddToStack();
        }

        private bool IsInSellIndicatorRange()
        {
            return IsInDistanceWith(SellArea.InteractPosition, interactionDistance);
        }

        private bool HasHigherPriorityForCollectArea()
        {
            return !HasHigherPriorityForDiscriminatorCollectArea();
        }

        private bool HasNoStack() => StackCount <= 0;

        private bool HasHigherPriorityForDiscriminatorCollectArea()
        {
            return  _currentArea.PieceDiscriminatorEndArea != null && GetDiscriminatorEndAreaStashCount() >= 10;
        }

        private int GetDiscriminatorEndAreaStashCount()
        {
            return _currentArea.PieceDiscriminatorEndArea.Stash.GetStachCount();
        }

        private bool HasStackOnDiscriminatorEndArea()
        {
            return GetDiscriminatorEndAreaStashCount() > 0;
        }

        // Enter Move TO Collect
        public void OnEnterMoveToCollectState()
        {
            Stop(false);
        }

        private const float interactionDistance = .4f;

        // Update Move To Collect
        public void OnUpdateMoveToCollectState()
        {
            if (!IsInDistanceWith(CollectArea.InteractPosition, interactionDistance))
            {
                _agent.SetDestination(CollectArea.InteractPosition);
            }
        }

        public void OnEnterCheckForCollectState()
        {
            _elapsedCheckTime = 0f;
            _currentCheckTime = 5f;
            Stop(true);
        }
        
        private float _elapsedCheckTime = 0f;
        private float _currentCheckTime;
        public void OnUpdateCheckForCollectState()
        {
            if (_elapsedCheckTime >= _currentCheckTime) return;

            _elapsedCheckTime += Time.deltaTime;

            if (_elapsedCheckTime >= _currentCheckTime)
            {
                bubble.Activate();
                bubble.SetText("We need more pieces boss!");
            }
        }

        public void OnExitCheckForCollectState()
        {
            bubble.Deactivate();
        }

        // Enter Collect
        public void OnEnterCollectState()
        {
            CollectArea.Interact(new InteractArgs(this));
        }

        // Enter Move To Sell
        public void OnEnterMoveToSellState()
        {
            Stop(false);
        }

        public void OnUpdateMoveToSellState()
        {
            _agent.SetDestination(SellArea.InteractPosition);
        }

        // Enter sell
        public void OnEnterSellState()
        {
            _elapsedCheckTime = 0f;
            _currentCheckTime = 4f;
            Stop(true);
            SellArea.Interact(new InteractArgs(this));
        }

        public void OnUpdateSellState()
        {
            if (_elapsedCheckTime >= _currentCheckTime) return;

            _elapsedCheckTime += Time.deltaTime;

            if (_elapsedCheckTime >= _currentCheckTime)
            {
                bubble.Activate();
                bubble.SetText("We need more pieces boss!");
            }
        }

        public void OnExitSellState()
        {
            bubble.Deactivate();
        }

        private bool IsInDistanceWith(Vector3 pos, float distance)
        {
            return Vector3.Distance(transform.position, pos) < distance;
        }

        public override void Stop(bool s)
        {
            _agent.isStopped = s;

            workerAnimations.SetMovementAnim(s ? 0f : 1f);
        }

        private void InitializeInteractions(AreaController currentArea)
        {
            _currentArea = currentArea;
            CollectArea = _currentArea.PieceCollectArea;
            SellArea = _currentArea.PieceSellArea;
        }

        public void ApplyStats()
        {
            maxStackSize = _data.CurrentStackSize;
            movementSpeed = _data.CurrentMoveSpeed;
            _agent.speed = movementSpeed;
        }

        // One AI will be working as demolisher after unlocking an area.
        public void CreateDemolishStates()
        {
            AIMoveToDemolishAreaState goToDemolishState = new AIMoveToDemolishAreaState(this);
            AIDemolishState demolishState = new AIDemolishState(this);

            Machine.RemoveTransition(MoveToCollectState);
            Machine.RemoveTransition(CheckCollectPiecesState);
            Machine.RemoveTransition(SellPiecesState);

            Machine.AddTransition(SellPiecesState, goToDemolishState, () => HasNoStack() && CanMoveToDemolish());
            Machine.AddTransition(MoveToCollectState, goToDemolishState, CanMoveToDemolish);
            Machine.AddTransition(CheckCollectPiecesState, goToDemolishState, CanMoveToDemolish);
            Machine.AddTransition(goToDemolishState, demolishState, CanDemolish);
        }

        public void CreateDiscriminatorStates()
        {
            AIMoveToSellDiscriminator goToSellDiscriminator = new AIMoveToSellDiscriminator(this);
            AISellDiscriminator sellDiscriminator = new AISellDiscriminator(this);
            AIMoveToCollectDiscriminator moveToCollectDiscriminator = new AIMoveToCollectDiscriminator(this);
            AICollectDiscriminator collectDiscriminator = new AICollectDiscriminator(this);

            // Clear collect piece state
            Machine.RemoveTransition(CollectPiecesState);
            
            // After collected & Go to sell pieces to discriminator
            Machine.AddTransition(CollectPiecesState, goToSellDiscriminator, CollectedEnoughFromCollectArea);

            // When arrived, transition into sell pieces.
            Machine.AddTransition(goToSellDiscriminator, sellDiscriminator, CanSellToDiscriminatorStartArea);
            
            // After selling, we have 2 conditions; If discriminate area has more than 10, go to discriminator collection area, if not, go default collect area
            Machine.AddTransition(sellDiscriminator, MoveToCollectState, () => HasNoStack() && HasHigherPriorityForCollectArea());
            Machine.AddTransition(sellDiscriminator, moveToCollectDiscriminator, () => HasNoStack() && HasHigherPriorityForDiscriminatorCollectArea());
            
            // When moved & arrived to discriminator end area; Start collecting pieces.
            Machine.AddTransition(moveToCollectDiscriminator, collectDiscriminator, CanCollectDiscriminatorEndArea);
            
            // After collecting, move to sell area..
            Machine.AddTransition(collectDiscriminator, MoveToSellState, CollectedEnoughFromDiscriminatorCollectArea);
            
            // If there is no stack on collect area and has stacks on dicriminator collect area,  go to there.
            Machine.AddTransition(CheckCollectPiecesState, moveToCollectDiscriminator, () => !HasStackOnCollectArea() && HasStackOnDiscriminatorEndArea());
            
            // AFter sold pieces, check if there are any stacks in discriminator stack area still.
            Machine.AddTransition(SellPiecesState, moveToCollectDiscriminator, () => HasNoStack() && HasStackOnDiscriminatorEndArea());
        }

        public void OnEnterMoveToCollectDiscriminatorEnd()
        {
            Stop(false);
        }
        
        public void OnUpdateMoveToCollectDiscriminatorEnd()
        {
            if (!IsInDistanceWith(_currentArea.PieceDiscriminatorEndArea.InteractPosition, interactionDistance))
            {
                _agent.SetDestination(_currentArea.PieceDiscriminatorEndArea.InteractPosition);
            }
        }

        public void OnEnterCollectDiscriminatorEnd()
        {
            _currentArea.PieceDiscriminatorEndArea.Interact(new InteractArgs(this));
        }

        private bool CanSellToDiscriminatorStartArea()
        {
            return IsInDistanceWith(_currentArea.PieceDiscriminatorStartArea.InteractPosition, interactionDistance);
        }
        
        private bool CanCollectDiscriminatorEndArea()
        {
            return IsInDistanceWith(_currentArea.PieceDiscriminatorEndArea.InteractPosition, interactionDistance);
        }

        public void OnEnterMoveToSellDiscriminatorStartState()
        {
            Stop(false);
        }
        
        public void OnUpdateMoveToSellDiscriminatorStartState()
        {
            if (!IsInDistanceWith(_currentArea.PieceDiscriminatorStartArea.InteractPosition, interactionDistance))
            {
                _agent.SetDestination(_currentArea.PieceDiscriminatorStartArea.InteractPosition);
            }
        }

        public void OnEnterSellDiscriminatorStartState()
        {
            Stop(true);
            _currentArea.PieceDiscriminatorStartArea.Interact(new InteractArgs(this));
        }

        private bool CanMoveToDemolish() => true;

        private bool CanDemolish() =>
            IsInDistanceWith(_currentArea.PieceDemolishArea.InteractPosition, interactionDistance);
        
        public override void TryDemolish(CraneArea area)
        {
            base.TryDemolish(area);

            float elapsed = 0f;
            float demolishInterval = 2f;
            _currentArea.DisableDemolishIndicatorCollision();
            model.SetActive(false);

            StartCoroutine(IE_Demolish());

            IEnumerator IE_Demolish()
            {
                while (enabled)
                {
                    while (elapsed < demolishInterval)
                    {
                        elapsed += Time.deltaTime;
                        yield return null;
                    }

                    demolishInterval = Random.Range(.8f, 2f);
                    elapsed = 0f;
                    _currentArea.TransferPiecesToCollectIndicator(_currentArea.BrickSpawnChance);
                    yield return null;
                }
            }
        }

        public void OnEnterMoveToDemolishState()
        {
            Stop(false);
        }

        public void OnUpdateMoveToDemolishState()
        {
            _agent.SetDestination(_currentArea.PieceDemolishArea.InteractPosition);
        }

        public void OnEnterDemolishState()
        {
            Stop(true);
            _currentArea.PieceDemolishArea.Interact(new InteractArgs(this));
            _currentArea.PieceDemolishArea.CanInteract = false;
        }
    }
}