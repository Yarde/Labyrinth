using UnityEngine;

namespace Labirynth
{
    public class Floor : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
    
        [SerializeField] private Material visitedMaterial;
        [SerializeField] private Material notVisitedMaterial;
    
        public void MarkVisited()
        {
            meshRenderer.material = visitedMaterial;
        }
    }
}