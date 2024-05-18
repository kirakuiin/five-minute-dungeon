using UnityEngine;

namespace Test
{
    [ExecuteAlways]
    public class Test : MonoBehaviour
    {
        [SerializeField] private MeshRenderer render;
        private static readonly int Position = Shader.PropertyToID("_Position");
        private static readonly int Forward = Shader.PropertyToID("_Forward");

        private void Update()
        {
            render.sharedMaterial.SetVector(Position, transform.position);
            render.sharedMaterial.SetVector(Forward, transform.up);
        }
    }
}
