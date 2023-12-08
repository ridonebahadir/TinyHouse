using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class TutorialLine : MonoBehaviour
    {
        [SerializeField] private LineRenderer lRenderer;

        private Transform _startPoint;
        private Vector3 _endPoint;

        public TutorialLine InitializeLine(Transform startPoint, Vector3 endPoint)
        {
            Activate();
            _startPoint = startPoint;
            _endPoint = endPoint;
            lRenderer.positionCount = 2;
            return this;
        }

        private void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Vector3.Distance(_startPoint.position, _endPoint) < 1.5f) Deactivate();
            lRenderer.SetPosition(0, _startPoint.position);
            lRenderer.SetPosition(1, _endPoint);
        }
    }
}