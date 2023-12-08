using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game_.Scripts.Shapes
{
    public class GridSpawner : MonoBehaviour
    {
        [Title("Grid Properties")] 
        [SerializeField] private int width = 2;
        [SerializeField] private int depth = 2;
        [SerializeField] private int height = 2;

        [SerializeField] private float xSpaceOffset = 1f;
        [SerializeField] private float zSpaceOffset = 1f;
        [SerializeField] private float ySpaceOffset = 1f;

        [SerializeField] private Transform gridParent;
        

        [Button("Spawn Grid withoutHeight")]
        private void SpawnGridWithoutHeight()
        {
            int idx = 1;
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    var position = transform.position + new Vector3(x * xSpaceOffset, 0f, z * zSpaceOffset);
                    var spawned = new GameObject($"Grid Point: {idx}");
                    spawned.transform.position = position;
                    spawned.transform.SetParent(gridParent);
                    idx++;

#if UNITY_EDITOR
                    spawned.AddComponent<PointGizmosDrawer>();
#endif
                }
            }
        }

        [Button("Spawn Grid with Height")]
        private void SpawnGridWithHeight()
        {

            int idx = 1;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        var position = transform.position + new Vector3(x * xSpaceOffset - width / 2f + (xSpaceOffset / 2f), y * ySpaceOffset, z * zSpaceOffset - depth / 2f + (zSpaceOffset / 2f));
                        var spawned = new GameObject($"Grid Point: {idx}");
                        spawned.transform.position = position;
                        spawned.transform.SetParent(gridParent);
                        idx++;

#if UNITY_EDITOR
                        spawned.AddComponent<PointGizmosDrawer>();
#endif
                    }
                }
            }
        }
        
    }
}