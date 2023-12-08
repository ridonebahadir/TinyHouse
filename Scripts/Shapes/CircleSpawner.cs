using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game_.Scripts.Shapes
{
    public class CircleSpawner : MonoBehaviour
    {
        private const float TAU = Mathf.PI * 2f;

        [SerializeField] protected int amountToSpawn = 10;
        [SerializeField] private float radius = 1f;
        public List<Transform> Points { get; set; } = new List<Transform>();

        [Button("Spawn Circle")]
        public void SpawnCircle()
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                var angle = (i * TAU) / amountToSpawn;
                var x = Mathf.Cos(angle) * radius;
                var z = Mathf.Sin(angle) * radius;

                Vector3 pos = transform.position + new Vector3(x, 0f, z);

                GameObject targetPoint = new GameObject("Circle Point " + i);
                targetPoint.transform.position = pos;
                targetPoint.transform.SetParent(transform);
                Points.Add(targetPoint.transform);

#if UNITY_EDITOR
                targetPoint.AddComponent<PointGizmosDrawer>();
#endif
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (radius <= 0 || amountToSpawn <= 0) return;

            for (int i = 0; i < amountToSpawn; i++)
            {
                var angle = (i * TAU) / amountToSpawn;
                var x = Mathf.Cos(angle) * radius;
                var z = Mathf.Sin(angle) * radius;

                Vector3 pos = transform.position + new Vector3(x, 0f, z);
                Gizmos.color = Color.red;

                Gizmos.DrawWireSphere(pos, .2f);
            }
        }
    }
}