using UnityEngine;

namespace Scripts.Effects
{
    [DisallowMultipleComponent]
    public class SpacetimeDeformer : MonoBehaviour
    {
        [Header("Ripple")]
        [SerializeField] private float _rippleSpeed = 1.57f;
        [SerializeField] private float _rippleRadius = 0.8f;
        [SerializeField] private float _rippleFrequency = 50.6f;
        [SerializeField] private float rippleStrength = 0.02f;

        private Mesh _mesh;
        private Vector3[] _originalVertices;
        private Vector3[] _deformedVertices;
        private float _time = 0f;

        private void Start()
        {
            // _mesh must be grabbed at runtime rather than dragged in inspector,
            // so the original _mesh isnt modified
            _mesh = GetComponent<MeshFilter>().mesh;

            // original verticies needed for calculation reference
            _originalVertices = _mesh.vertices;
            _deformedVertices = new Vector3[_originalVertices.Length];
        }

        private void Update()
        {
            _time += Time.deltaTime;

            for (int i = 0; i < _originalVertices.Length; i++)
            {
                Vector3 ogVerts = _originalVertices[i];
                float dist = ogVerts.magnitude; // distance from center

                float rippleRad = Mathf.Clamp01(1f - (dist / _rippleRadius));
                rippleRad = rippleRad * rippleRad;  // ripple is more pronounced in the center

                // Wave rings travel outward from the center
                float ripple = Mathf.Sin(dist * _rippleFrequency - _time * _rippleSpeed) * rippleStrength * rippleRad;

                _deformedVertices[i] = ogVerts + transform.up * (ripple);
            }

            // apply newly calculated mesh
            _mesh.vertices = _deformedVertices;
            _mesh.RecalculateNormals();
        }
    }
}