using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.Scripts
{
    public abstract class BaseBuilding : MonoBehaviour, IInteractable, IAreaElement
    {
        [SerializeField] protected float interactionCooldown = 2f;
        [SerializeField] private TextMeshProUGUI indicatorText;
        
        private string defaultText;
        
        private BaseWorker _interactor;
        public Vector3 InteractPosition { get; }
        public bool CanInteract { get; set; }


        private Coroutine _cooldownRoutine;

        private void InterruptCooldown()
        {
            if (_cooldownRoutine == null) return;
            SetIndicatorText(defaultText);
            StopCoroutine(_cooldownRoutine);
            _cooldownRoutine = null;
        }

        private void Start()
        {
            ScaleUp();
        }

        public void ScaleUp()
        {
            Vector3 initialScale = transform.localScale;
            transform.localScale = Vector3.zero;
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(initialScale * 1.2f, .3f));
            seq.Append(transform.DOScale(initialScale,.3f));
        }

        protected void SetCooldown(Action OnComplete = null)
        {
            defaultText = indicatorText.text;

            float cd = interactionCooldown;

            _cooldownRoutine = StartCoroutine(IE_SetCooldown());

            IEnumerator IE_SetCooldown()
            {
                while (cd > 0f)
                {
                    cd -= Time.deltaTime;
                    indicatorText.text = $"{cd:0.0}";
                    yield return null;
                }

                OnComplete?.Invoke();
                SetIndicatorText(defaultText);
            }
        }
        
        private void SetIndicatorText(string text)
        {
            indicatorText.text = text;
        }
        
        public virtual void Interact(InteractArgs args)
        {
            // Set worker as interactor
            _interactor = args.Worker;
            // Stop interactor
            _interactor.Stop(true);

            // Subscribe to close event to allow user to move again
  
        }
        
        protected virtual void AllowInteractorToMove()
        {
            _interactor.Stop(false);
        }

        public virtual void StopInteract(InteractArgs args)
        {
            InterruptCooldown();
        }
        public string AreaID { get; set; }
        public AreaController AssignedArea { get; set; }
        public void InitArea(AreaController area)
        {
            AssignedArea = area;
            AreaID = area.AreaID;
        }
    }
}