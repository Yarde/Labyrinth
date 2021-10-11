﻿using System.Collections.Generic;
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
        private Player _player;

        public void Update()
        {
            while (currentHearts > _player.Hearts)
            {
                RemoveHeart();
            }
            while (currentHearts < _player.Hearts)
            {
                AddHeart();
            }
        }
        
        public void SetupBar(Player player)
        {
            spawnedHearts = new Stack<GameObject>();
            _player = player;
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