using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    public class Discriminator : BaseBuilding
    {
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform discriminatePoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private float discriminateDelay = 1f;
        [SerializeField] private Transform buildingIndicatorPoint;
        
        private PieceType typeToDiscriminate;

        [SerializeField] private ResourceSo resource;

        public IndicatorDiscriminatorStart StartIndicator { get; set; }
        public IndicatorDiscriminatorEnd EndIndicator { get; set; }

        public Transform BuildingIndicatorPoint => buildingIndicatorPoint;

        private WaitForSeconds _discriminateDelayInWfs;

        [Title("BlendShape")] private bool countingUp = true;
        private int value = 0;
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

        private int costIdx = 0;

        private int _currentUpgradeIndex;

        private List<PieceData> upgradePieceDatas;

        public void InitializeDiscriminator(PieceData[] data, PieceType typeToDiscriminate, AreaController area,
            IndicatorDiscriminatorStart start, IndicatorDiscriminatorEnd end)
        {
            // Set Indicators
            InitializeUpgradeDatas(data);
            this.typeToDiscriminate = typeToDiscriminate;

            StartIndicator = start;
            EndIndicator = end;
            // Initialize Area
            InitArea(area);

            //
            StartIndicator.InitArea(area);
            EndIndicator.InitArea(area);


            _discriminateDelayInWfs = new WaitForSeconds(discriminateDelay);
            _currentUpgradeIndex = ES3.Load($"{gameObject.name}_discriminator_upgradePieceData", 0);
            StartCoroutine(IE_TryDiscriminate());
        }

        private DiscriminatorUpgradeUI _ui;

        public void InitBuilding(DiscriminatorUpgradeUI ui, int defaultIdx)
        {
            _ui = ui;
            costIdx = defaultIdx;
        }

        private void InitializeUpgradeDatas(PieceData[] data)
        {
            upgradePieceDatas = new List<PieceData>(data);
        }

        public bool IncreaseUpgradeIndex()
        {
            if (CanUpgrade())
            {
                var resourceCost = GetResourceCost();
                if (resource.HasEnoughAmount(resourceCost))
                {
                    resource.Decrease(resourceCost);
                    _currentUpgradeIndex++;
                    costIdx++;
                    ES3.Save($"{gameObject.name}_discriminator_upgradePieceData", _currentUpgradeIndex);
                    return true;
                }

                return false;
            }

            Debug.Log("Can't upgrade more..");
            return false;
        }

        public int GetResourceCost()
        {
            int lastLevel = AssignedArea.AreaLevel;
            int resourceCost = (int)Formulas.GetDiscriminatorUpgradeCosts(costIdx, lastLevel);
            return resourceCost;
        }

        public bool CanUpgrade()
        {
            return _currentUpgradeIndex < upgradePieceDatas.Count - 1;
        }

        private PieceData GetCurrentUpgradeData()
        {
            return upgradePieceDatas[_currentUpgradeIndex];
        }

        public string GetNextUpgradeName()
        {
            return CanUpgrade() ? upgradePieceDatas[_currentUpgradeIndex + 1].pieceType.ToString() : "Completed";
        }

        IEnumerator IE_TryDiscriminate()
        {
            while (enabled)
            {
                if (StartIndicator.Stash.GetStachCount() > 0)
                {
                    yield return _discriminateDelayInWfs;
                    if (StartIndicator.Stash.GetStachCount() > 0 && StartIndicator.GetStash().PieceType == typeToDiscriminate) Discriminate(StartIndicator.RemoveStash());
                }

                yield return null;
            }
        }

        public override void Interact(InteractArgs args)
        {
            SetCooldown(() =>
            {
                base.Interact(args);
                _ui.InitUI(this);
                _ui.Open();
                _ui.OnCloseUI += AllowInteractorToMove;
            });
        }


        protected override void AllowInteractorToMove()
        {
            base.AllowInteractorToMove();
            _ui.OnCloseUI -= AllowInteractorToMove;
        }

        private void Discriminate(DemolishPiece piece)
        {
            int lastLevel = AssignedArea.AreaLevel;

            StartCoroutine(IE_Discriminate());

            IEnumerator IE_Discriminate()
            {
                var jumpTween = piece.Jump(startPoint.position);

                yield return jumpTween.WaitForCompletion();

                var moveToDiscriminatePointTween = piece.Move(discriminatePoint.position, 3f).SetEase(Ease.Linear);

                yield return moveToDiscriminatePointTween.WaitForCompletion();
                // Dicriminate piece by setting its worth and type

                piece.SetPieceDataByType(GetCurrentUpgradeData().pieceType,
                    (int)Formulas.GetGainFormula(_currentUpgradeIndex, lastLevel));

                // Start coroutine for blend shape here.
                StartCoroutine(BlendShapeObject());

                var moveTween = piece.Move(endPoint.position, 3f).SetEase(Ease.Linear);

                yield return moveTween.WaitForCompletion();
                
                EndIndicator.AddDemolishPieceToStach(piece);

            }
        }

        private IEnumerator BlendShapeObject()
        {
            while (true)
            {
                if (countingUp)
                {
                    if (value < 100)
                    {
                        value++;
                        skinnedMeshRenderer.SetBlendShapeWeight(0, value);
                    }
                    else
                    {
                        countingUp = false;
                    }
                }
                else
                {
                    if (value > 0)
                    {
                        value--;
                        skinnedMeshRenderer.SetBlendShapeWeight(0, value);
                    }
                    else
                    {
                        countingUp = true;
                        yield break;
                    }
                }

                yield return null;
            }
        }
    }
}