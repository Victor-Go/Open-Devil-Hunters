using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
    public class RadarChart : MonoBehaviour
    {
        public Material material;

        private CanvasRenderer canvasRenderer;
        private List<float> percentages = new();

        private void Awake()
        {
            canvasRenderer = GetComponent<CanvasRenderer>();

            percentages = new()
            {
                0.2f,
                0.7f,
                0.3f,
                0.9f,
                1
            };
        }

        public void SetPercentages(List<float> percentages)
        {
            this.percentages = percentages;
        }

        private Vector3[] GetPoints()
        {
            return new Vector3[]{
                Vector3.zero,
                (Vector3.up * 150) * percentages[0],
                Quaternion.AngleAxis(-72, Vector3.forward) * (Vector3.up * 140) * percentages[1],
                Quaternion.AngleAxis(-72 * 2, Vector3.forward) * (Vector3.up * 140) * percentages[2],
                Quaternion.AngleAxis(-72 * 3, Vector3.forward) * (Vector3.up * 140) * percentages[3],
                Quaternion.AngleAxis(-72 * 4, Vector3.forward) * (Vector3.up * 140) * percentages[4],
            };
        }

        private void Start()
        {
            Mesh mesh = new Mesh();

            mesh.vertices = GetPoints();
            mesh.triangles = new[] {
                0, 1, 2,
                2, 0, 3,
                3, 0, 4,
                4, 0, 5,
                5, 0, 1
            };

            canvasRenderer.SetMesh(mesh);
            canvasRenderer.SetMaterial(material, null);
        }
    }
}
