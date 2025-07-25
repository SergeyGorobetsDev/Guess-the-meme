using System.Collections.Generic;
using UnityEngine;

namespace Assets.Project.Code.Gameplay.Actors.Spawner
{
    public sealed class SpawnArea : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        public float circleRadius = 1f;
        [SerializeField]
        private float spacing = 0.1f;

        public List<Vector3> SpawnPositions(float radius = 0)
        {
            List<Vector3> positions = new List<Vector3>();
            float minDistance = (radius * 2) + spacing;

            positions.Add(Vector3.zero);

            for (float r = minDistance; r <= circleRadius; r += minDistance)
            {
                float circumference = 2 * Mathf.PI * r;
                int numObjects = Mathf.FloorToInt(circumference / minDistance);

                if (numObjects > 0)
                {
                    float angleStep = 360f / numObjects;

                    for (int i = 0; i < numObjects; i++)
                    {
                        float angle = i * angleStep;
                        Vector3 position = new Vector3(
                            r * Mathf.Cos(angle * Mathf.Deg2Rad),
                            0f,
                            r * Mathf.Sin(angle * Mathf.Deg2Rad)
                        );
                        positions.Add(position);
                    }
                }
            }

            return positions;
        }


#if UNITY_EDITOR

        [Header("Debug")]
        [SerializeField]
        private float testRadius = 0.5f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, circleRadius);

            List<Vector3> positions = SpawnPositions(testRadius);
            Gizmos.color = Color.red;
            foreach (Vector3 pos in positions)
            {
                Gizmos.DrawWireSphere(transform.position + pos, testRadius);
            }
        }
#endif
    }
}
