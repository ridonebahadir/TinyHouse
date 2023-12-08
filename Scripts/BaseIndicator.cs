using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace _Project.Scripts
{

    public abstract class BaseIndicator : MonoBehaviour, IAreaElement
    {
        [Title("Unlock Status")] 
        [SerializeField] protected bool unlockedIndicatorAsDefault;
        [SerializeField] protected bool unlockedBuildingAsDefault;
        
        [SerializeField] protected float interactionCooldown = 2f;
        [SerializeField] private TextMeshProUGUI indicatorText;
        [SerializeField] private GameObject model;
        
        private string defaultText;
        private Collider _collider;

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider>();
            defaultText = indicatorText.text;
        }

        protected virtual void Start()
        {
            SetYAxisPosition();
        }

        private void SetYAxisPosition()
        {
            transform.localPosition = new Vector3(transform.localPosition.x, .1f, transform.localPosition.z);
        }

        protected Coroutine CooldownRoutine;
        protected Coroutine InteractRoutine;

        protected void StopInteractRoutine()
        {
            if (InteractRoutine == null) return;
            StopCoroutine(InteractRoutine);
            InteractRoutine = null;
        }
        
        protected void SetCooldown(Action OnComplete = null)
        {
            float cd = interactionCooldown;

            CooldownRoutine = StartCoroutine(IE_SetCooldown());

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

        public virtual void Activate()
        {
            gameObject.SetActive(true);
            UnlockedIndicator = true;
        }
        
        public void TryLoadIndicatorData()
        {
            if (UnlockedIndicator) Activate();
            else gameObject.SetActive(false);
        }

        public void DisableCollider()
        {
            _collider.enabled = false;
            model.SetActive(false);
        }

        protected void InterruptCooldown()
        {
            if (CooldownRoutine == null) return;
            SetIndicatorText(defaultText);
            StopCoroutine(CooldownRoutine);
            CooldownRoutine = null;
        }

        private void SetIndicatorText(string text)
        {
            indicatorText.text = text;
        }

        public string AreaID { get; set; }
        public AreaController AssignedArea { get; set; }

        public virtual void InitArea(AreaController area)
        {
            AssignedArea = area;
            AreaID = area.AreaID;
        }
        
        private const string IndicatorData = "_unlockedIndicator";
        private const string BuildingData = "_unlockedBuilding";

        public bool UnlockedIndicator
        {
            get => ES3.Load($"{AreaID}{IndicatorData}{gameObject.name}", unlockedIndicatorAsDefault);
            set => ES3.Save($"{AreaID}{IndicatorData}{gameObject.name}", value);
        }
        
        public bool UnlockedIndicatorTargetObject
        {
            get => ES3.Load($"{AreaID}{BuildingData}{gameObject.name}", unlockedBuildingAsDefault);
            set => ES3.Save($"{AreaID}{BuildingData}{gameObject.name}", value);
        }
    }
}