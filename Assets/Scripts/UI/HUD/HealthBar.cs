using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject heartObject;
        [SerializeField] private HorizontalLayoutGroup holder;

        // todo optimization
        // it can be solved with object pool
        private Stack<GameObject> spawnedHearts;
        private int currentHearts = 0;

        private void Start()
        {
            spawnedHearts = new Stack<GameObject>();
        }

        private void UpdateBar(int numberOfHearts)
        {
            while (currentHearts > numberOfHearts)
            {
                RemoveHeart();
            }
            while (currentHearts < numberOfHearts)
            {
                AddHeart();
            }
        }

        private void AddHeart()
        {
            var image = Instantiate(heartObject, holder.transform);
            spawnedHearts.Push(image);
            currentHearts++;
        }
        
        private void RemoveHeart()
        {
            var image = spawnedHearts.Pop();
            Destroy(image);
            currentHearts--;
        }
    }
}