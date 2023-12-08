using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts
{
    public class ResourceManager : SingletonClass.Singleton<ResourceManager>
    {
        [SerializeField] private DemolishPiece testDemolishPiecePrefab;
        [SerializeField] private GameObject moneyPiece;
        [SerializeField] private GameObject dustParticle;
        

        private PoolSystem<PoolableObj> _demolishPiecePool;
        private PoolSystem<PoolableObj> _moneyPiecePool;
        private PoolSystem<PoolableObj> _dustParticlePool;

        protected override void Awake()
        {
            base.Awake();
            _demolishPiecePool = new PoolSystem<PoolableObj>(testDemolishPiecePrefab.gameObject, 10, transform);
            _moneyPiecePool = new PoolSystem<PoolableObj>(moneyPiece, 10, transform);
            _dustParticlePool = new PoolSystem<PoolableObj>(dustParticle, 5, transform);
        }

        public DemolishPiece SpawnDemolishPiece(Vector3 position)
        {
            DemolishPiece spawnedPiece = _demolishPiecePool.Pull(position, Quaternion.identity).GetComponent<DemolishPiece>();
            spawnedPiece.transform.SetParent(transform);
            return spawnedPiece;
        }
        
        public GameObject SpawnDustParticle(Vector3 position)
        {
            var spawnedPiece = _dustParticlePool.PullGameObject(position, Quaternion.identity);
            spawnedPiece.transform.SetParent(transform);
            return spawnedPiece;
        }
        
        public GameObject SpawnMoneyPiece(Vector3 position)
        {
            GameObject spawnedPiece = _moneyPiecePool.PullGameObject(position, Quaternion.identity);
            spawnedPiece.transform.SetParent(transform);
            Vector3 initialScale = moneyPiece.transform.localScale;
            spawnedPiece.transform.localScale = Vector3.zero;
            spawnedPiece.transform.DOScale(initialScale, .2f);
            return spawnedPiece;
        }
    }
}