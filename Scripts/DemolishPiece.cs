using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts
{
    public enum PieceType
    {
        Brick,
        Gold,
        Diamond,
        Ytong,
        Iron,
        Steel,
        Silver,
        Platinum
    }

    public class DemolishPiece : MonoBehaviour
    {
        [Title("Piece Models")]
        [SerializeField] private GameObject brickModel;
        [SerializeField] private GameObject goldModel;
        [SerializeField] private GameObject diamondModel;
        [SerializeField] private GameObject ytongModel;
        [SerializeField] private GameObject ironModel;
        [SerializeField] private GameObject steelModel;
        [SerializeField] private GameObject silverModel;
        [SerializeField] private GameObject platinumModel;

        [Title("Durations")]
        [SerializeField] private float moveDuration = .3f;

        [Title("Default Piece Data")] [SerializeField]
        private PieceData defaultPieceData;

        private PieceData pieceData;
        private GameObject _currentModel;
        
        [Title("Brick Color")]
        [SerializeField] private Color brickcolor1;
        [SerializeField] private Color brickcolor2;
        [SerializeField] private Color brickcolor3;
        
        [Title("Ytong Color")]
        [SerializeField] private Color ytongColor1;
        [SerializeField] private Color ytongColor2;
        [SerializeField] private Color ytongColor3;
        
        
        public float ySize { get; private set; }
        public int PieceWorth => pieceData.pieceWorth;
        public PieceType PieceType => pieceData.pieceType;

        private void Start()
        {
            // Set ySize
            ySize = 0.4f;
        }
        
        public void SetPieceDataByType(PieceType type, int worth)
        {
            if (_currentModel == null) _currentModel = brickModel;
            pieceData = new PieceData(type, worth);
            ChangeModel(GetPieceModelByType(type));
        }
        
        private void ChangeModel(GameObject obj)
        {
            _currentModel.gameObject.SetActive(false);
            _currentModel = obj;
            _currentModel.gameObject.SetActive(true);
        }

        private GameObject GetPieceModelByType(PieceType type)
        {
            return type switch
            {
                PieceType.Brick => brickModel,
                PieceType.Gold => goldModel,
                PieceType.Diamond => diamondModel,
                PieceType.Ytong => ytongModel,
                PieceType.Iron => ironModel,
                PieceType.Steel => steelModel,
                PieceType.Silver => silverModel,
                PieceType.Platinum => platinumModel,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public void RepositionLerpLocal(Vector3 position, Transform parent)
        {
            
            transform.SetParent(parent);
            transform.DOLocalJump(position, 1f, 1, moveDuration).OnComplete(() =>
            {
                transform.localRotation = Quaternion.identity;
            });
        }
        
        public Tween Jump(Vector3 position, Action OnArrive = null)
        {
            transform.localRotation = Quaternion.identity;
            return transform.DOJump(position, 4f, 1, moveDuration).OnComplete(() => OnArrive?.Invoke());
        }
        
        public Tween Move(Vector3 position, float duration, Action OnArrive = null)
        {
            transform.localRotation = Quaternion.identity;
            return transform.DOMove(position, duration).OnComplete(() => OnArrive?.Invoke());
        }
    }
}