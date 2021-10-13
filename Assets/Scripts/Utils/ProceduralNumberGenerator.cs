using UnityEngine;

namespace Utils
{
    public class ProceduralNumberGenerator
    {
        private static int _currentPosition = 0;
        private static string _key;

        public static void Initialize(int seed)
        {
            Random.InitState(seed);
            GenerateKey();
        }

        private static void GenerateKey()
        {
            for (var i = 0; i < 40; i++)
            {
                var nextRandom = (int) (Random.value * 4 + 1);
                _key += nextRandom;
            }
            //_key = "12342412123334242143223314412441212322344321212233344";
        }

        public static int GetNextNumber()
        {
            var currentNum = _key.Substring(_currentPosition++ % _key.Length, 1);
            return int.Parse(currentNum);
        }

        public static Vector2Int GetRandomCell(Vector2Int dimensions)
        {
            var x = (int) (Random.value * dimensions.x);
            var y = (int) (Random.value * dimensions.y);
            return new Vector2Int(x, y);
        }
    }
}