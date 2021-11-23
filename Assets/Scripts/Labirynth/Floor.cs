using UnityEngine;

namespace Labirynth
{
    public class Floor : MonoBehaviour
    {
        [SerializeField] private GameObject onVisit;
    
        public void MarkVisited()
        {
            onVisit.SetActive(true);
        }
    }
}