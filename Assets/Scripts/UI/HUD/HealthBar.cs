using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject heartObject;
        [SerializeField] private HorizontalLayoutGroup holder;
        
        private Stack<GameObject> _spawnedHearts;
        private int _currentHearts;
        private Player _player;

        public void Update()
        {
            while (_currentHearts > _player.Hearts)
            {
                _ = RemoveHeart();
            }
            while (_currentHearts < _player.Hearts)
            {
                AddHeart();
            }
        }
        
        public void SetupBar(Player player)
        {
            _spawnedHearts = new Stack<GameObject>();
            _player = player;
        }

        private void AddHeart()
        {
            var image = Instantiate(heartObject, holder.transform);
            _spawnedHearts.Push(image);
            _currentHearts++;
        }
        
        private async UniTask RemoveHeart()
        {
            _currentHearts--;
            var image = _spawnedHearts.Pop();
            
            await image.transform.DOShakeRotation(0.5f);
            Destroy(image);
        }
    }
}