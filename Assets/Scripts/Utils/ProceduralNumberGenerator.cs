using System.Collections.Generic;
using Labirynth;
using UnityEngine;

namespace Utils
{
    public static class ProceduralNumberGenerator
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

        public static GameObject GetRandomCell(List<GameObject> spawns)
        {
            var x = (int) (Random.value * spawns.Count);
            return spawns[x];
        }
        
        public static Cell GetRandomCell(List<Cell> cells)
        {
            var x = (int) (Random.value * cells.Count);
            return cells[x];
        }
        
        public static Vector3 GetRandomDirection(List<Vector3> directions)
        {
            var x = (int) (Random.value * directions.Count);
            return directions[x];
        }
        
        public static void Shuffle<T>(this IList<T> list)  
        {  
            var n = list.Count;  
            while (n > 1) {  
                n--;  
                var k = Random.Range(0, n + 1);  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
    }
}