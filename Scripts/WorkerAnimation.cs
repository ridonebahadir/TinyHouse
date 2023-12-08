using UnityEngine;

namespace _Project.Scripts
{
    public class WorkerAnimation
    {
        private readonly Animator _animator;
        
        private static readonly int moveMagnitude = Animator.StringToHash("moveMagnitude");

        public WorkerAnimation(Animator animator)
        {
            _animator = animator;
        }

        public void SetMovementAnim(float value)
        {
            _animator.SetFloat(moveMagnitude, value);
        }
    }
}