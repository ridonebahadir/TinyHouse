using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    
    public class BaseStash<T>
    {
        private readonly Stack<T> _stash = new Stack<T>();

        public Action<T> OnAddedStach;
        public Action<T> OnRemovedStach;
        public Action OnStachFull;
        
        public void AddToStach(T piece)
        {
            _stash.Push(piece);
            OnAddedStach?.Invoke(piece);
        }

        public T RemoveFromStach()
        {
            var poppedStash = _stash.Pop();
            OnRemovedStach?.Invoke(poppedStash);
            return poppedStash;
        }

        public T GetPieceFromStash()
        {
            var piece = _stash.Peek();
            return piece;
        }

        public bool HasStach()
        {
            return GetStachCount() > 0;
        }

        public int GetStachCount()
        {
            return _stash.Count;
        }
    }
    
    public class DemolishPieceStash : BaseStash<DemolishPiece> { }
    public class MoneyStash : BaseStash<GameObject> { }
}