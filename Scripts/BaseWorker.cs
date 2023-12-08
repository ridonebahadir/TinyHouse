using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts;
using DG.Tweening;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseWorker : MonoBehaviour
{
    [Title("References")] [SerializeField] private Animator workerAnimator;

    [Title("Worker Movement Settings")] [ReadOnly] [SerializeField]
    protected float movementSpeed;

    [SerializeField] protected float rotationSpeed = 720f;

    [Title("Stack Settings")] [SerializeField]
    private Transform stackSlot;

    [ReadOnly] [SerializeField] protected int maxStackSize;


    protected WorkerAnimation workerAnimations;

    protected DemolishPieceStash StackedPieces = new DemolishPieceStash();


    public Coroutine InteractRoutine;
    public int StackCount => StackedPieces.GetStachCount();

    protected virtual void Awake()
    {
        workerAnimations = new WorkerAnimation(workerAnimator);
    }

    protected virtual void Start()
    {
    }

    public virtual void Update()
    {
    }

    private void LateUpdate()
    {
        if (_activeSlots.Count > 0)
        {
            Transform firstSlot = _activeSlots[0];
            MoveSlot(firstSlot, stackSlot.position);
            RotateSlot(firstSlot, transform.rotation);

            for (int i = 1; i < _activeSlots.Count; i++)
            {
                MoveSlot(_activeSlots[i], _activeSlots[i - 1].position);
                RotateSlot(_activeSlots[i], _activeSlots[i - 1].rotation);
            }
        }
    }


    private void MoveSlot(Transform slotObj, Vector3 targetPos)
    {
        var position = new Vector3(targetPos.x, slotObj.transform.position.y, targetPos.z);
        Vector3 startPosition = slotObj.position;
        slotObj.position = Vector3.Lerp(startPosition, position, 55f * Time.smoothDeltaTime);
    }

    private void RotateSlot(Transform slotObj, Quaternion targetRotation)
    {
        Quaternion startRotation = slotObj.rotation;
        slotObj.rotation = Quaternion.Lerp(startRotation, targetRotation, 25f * Time.deltaTime);
    }


    private readonly List<Transform> _activeSlots = new List<Transform>();
    private Transform _slotsParent;

    public virtual void AddStack(DemolishPiece pieceToAdd)
    {
        // Create slot parent
        if (_slotsParent == null)
        {
            _slotsParent = new GameObject($"{gameObject.name} Slots").transform;
            _slotsParent.transform.position = Vector3.zero;
        }

        // Calculate slot position
        Vector3 calculatedPos = new Vector3(stackSlot.position.x, (pieceToAdd.ySize * StackedPieces.GetStachCount()),
            stackSlot.position.z);


        // If we dont have enough slot &spawn them
        if (_activeSlots.Count <= StackedPieces.GetStachCount())
        {
            var slot = new GameObject($"Stack Slot {_activeSlots.Count + 1}");
            slot.transform.position = calculatedPos;
            slot.transform.SetParent(_slotsParent);
            _activeSlots.Add(slot.transform);
        }

        pieceToAdd.transform.SetParent(_activeSlots[StackedPieces.GetStachCount()]);
        pieceToAdd.transform.DOLocalJump(Vector3.zero, 2.5f, 1, .5f);
        pieceToAdd.transform.DOLocalRotate(Vector3.zero, .2f);
        StackedPieces.AddToStach(pieceToAdd);
        PlayStackSound();

        if (!CanAddToStack()) StackedPieces.OnStachFull?.Invoke();
    }

    public virtual DemolishPiece RemoveStack()
    {
        DemolishPiece piece = StackedPieces.RemoveFromStach();
        PlayStackSound();
        return piece;
    }

    public virtual DemolishPiece GetStack()
    {
        DemolishPiece piece = StackedPieces.GetPieceFromStash();
        return piece;
    }

    public bool CanDiscriminate(PieceType[] types)
    {
        Debug.Log(GetStack() == null);
        return types.Any(x => x == GetStack().PieceType);
    }

    protected virtual void PlayStackSound()
    {
    }

    public bool CanAddToStack()
    {
        return StackedPieces.GetStachCount() < maxStackSize;
    }

    public void InterruptInteractRoutine()
    {
        if (InteractRoutine == null) return;
        StopCoroutine(InteractRoutine);
        InteractRoutine = null;
    }

    public virtual void TryDemolish(CraneArea area)
    {
        Stop(true);
    }

    public abstract void Stop(bool status);

    protected virtual void HandleMovement()
    {
    }
}