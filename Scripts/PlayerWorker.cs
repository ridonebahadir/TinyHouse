using System;
using System.Linq;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    public class PlayerWorker : BaseWorker
    {
        [SerializeField] private PlayerWorkerData data;
        [SerializeField] private TextMeshProUGUI statusText;

        [SerializeField] private LayerMask deadzoneMask;

        [Title("Sounds")] [SerializeField] private AudioClip stackClip;

        private bool _disableMovement = false;

        private Vector3 playerPos;

        private float SavedX;
        private float SavedZ;

        protected override void Start()
        {
            base.Start();
            playerPos = transform.position;
            SavedX = ES3.Load(nameof(SavedX), playerPos.x);
            SavedZ = ES3.Load(nameof(SavedZ), playerPos.z);
            Debug.Log(SavedX);
            transform.position = new Vector3(SavedX, playerPos.y, SavedZ);
            ApplyPlayerStats();
        }

        private void OnEnable()
        {
            data.OnCurrentStackSizeChanged += ApplyPlayerStats;
            data.OnCurrentMoveSpeedChanged += ApplyPlayerStats;
            StackedPieces.OnStachFull += SetStashUI;
            StackedPieces.OnRemovedStach += DisableStatusText;
        }

        private void OnDisable()
        {
            ES3.Save(nameof(SavedX), transform.position.x);
            ES3.Save(nameof(SavedZ), transform.position.z);
            
            data.OnCurrentStackSizeChanged -= ApplyPlayerStats;
            data.OnCurrentMoveSpeedChanged -= ApplyPlayerStats;
            StackedPieces.OnStachFull -= SetStashUI;
            StackedPieces.OnRemovedStach -= DisableStatusText;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact(new InteractArgs(this));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                interactable.StopInteract(new InteractArgs(this));
            }
        }

        public override void Update()
        {
            base.Update();
            HandleMovement();
        }

        public override void AddStack(DemolishPiece pieceToAdd)
        {
            base.AddStack(pieceToAdd);
            
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
        }

        public override DemolishPiece RemoveStack()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
            return base.RemoveStack();
        }

        private void SetStashUI()
        {
            SetStatusText(true, "Max");
        }

        private void SetStatusText(bool status, string text = "")
        {
            statusText.enabled = status;
            statusText.text = text;
        }

        private void DisableStatusText(DemolishPiece piece)
        {
            if (statusText.enabled) statusText.enabled = false;
        }

        private void ApplyPlayerStats()
        {
            maxStackSize = data.CurrentStackSize;
            movementSpeed = data.CurrentMoveSpeed;
        }

        public bool CheckDeadzone()
        {
            return Physics.Raycast(transform.position, transform.forward, 1f, deadzoneMask);
        }

        public override void Stop(bool status)
        {
            _disableMovement = status;
            workerAnimations.SetMovementAnim(_disableMovement ? 0f : 1f);
        }

        private float minxClamp, maxXClamp, minZClamp, maxZClamp;

        public void SetPlayerClampValues(float minX, float maxX, float minZ, float maxZ)
        {
            minxClamp = minX;
            maxXClamp = maxX;
            minZClamp = minZ;
            maxZClamp = maxZ;
            Debug.Log("Setting max clamp:" + maxXClamp);
        }

        private CraneArea _craneArea;

        public override void TryDemolish(CraneArea area)
        {
            base.TryDemolish(area);

            _craneArea = area;

            UIManager.Instance.ActivateBlackScreen(.6f, .6f, 1.5f, () => { area.Activate(true); },
                area.ActivateButtons);

            area.OnClickedExitArea += SetPlayerAfterDemolish;
        }

        private void SetPlayerAfterDemolish()
        {
            Stop(false);
            if (_craneArea != null) _craneArea.OnClickedExitArea -= SetPlayerAfterDemolish;
        }

        protected override void PlayStackSound()
        {
            SoundManager.Instance.PlaySoundWithPitch(stackClip, 1f + StackCount * .075f);
        }

        protected override void HandleMovement()
        {
            if (_disableMovement) return;

            base.HandleMovement();
            var inputVector = MobileJoystick.Instance.GetJoystickVector(0f).normalized;

            if (!CheckDeadzone())
            {
                // Move
                transform.Translate(inputVector * (movementSpeed * Time.deltaTime), Space.World);
                // Clam position
                /*var position = transform.position;
                position = new Vector3(Mathf.Clamp(position.x, minxClamp, maxXClamp), position.y, Mathf.Clamp(position.z, minZClamp, maxZClamp));
                transform.position = position;*/
            }

            // Rotate
            if (inputVector != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(inputVector, Vector3.up);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }

            // Animation
            workerAnimations.SetMovementAnim(inputVector.magnitude);
        }
    }
}