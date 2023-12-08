using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseBuildingUI : MonoBehaviour
    {
        [SerializeField] protected Button closeButton;
        [SerializeField] private CanvasGroup uiReference;
        
        public Action OnCloseUI;
        public Action OnOpenUI;
        
        protected virtual void Start()
        {
            InitialClose();
        }

        protected virtual void OnEnable()
        {
            closeButton.onClick.AddListener(Close);
        }

        protected void OnDisable()
        {
            closeButton.onClick.RemoveListener(Close);
        }

        public virtual void Open()
        {
            uiReference.alpha = 1f;
            uiReference.interactable = true;
            uiReference.blocksRaycasts = true;
            OnOpenUI?.Invoke();
        }

        private void InitialClose()
        {
            uiReference.alpha = 0f;
            uiReference.interactable = false;
            uiReference.blocksRaycasts = false;
        }
        public virtual void Close()
        {
            uiReference.alpha = 0f;
            uiReference.interactable = false;
            uiReference.blocksRaycasts = false;
            OnCloseUI?.Invoke();
        }
        
    }
}