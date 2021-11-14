using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject heartObject;
        [SerializeField] private HorizontalLayoutGroup holder;

        // todo optimization it can be solved with object pool
        private Stack<GameObject> spawnedHearts;
        private int currentHearts = 0;
        private Player.Player _player;

        public void Update()
        {
            while (currentHearts > _player.Hearts)
            {
                _ = RemoveHeart();
            }
            while (currentHearts < _player.Hearts)
            {
                AddHeart();
            }
        }
        
        public void SetupBar(Player.Player player)
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
        
        private async UniTask RemoveHeart()
        {
            currentHearts--;
            var image = spawnedHearts.Pop();
            
            await image.transform.DOShakeRotation(0.5f);
            Destroy(image);
        }
    }
}