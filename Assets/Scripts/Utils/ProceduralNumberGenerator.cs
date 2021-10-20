﻿using System.Collections.Generic;
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

        public static Vector2Int GetRandomCell(Vector2Int dimensions)
        {
            var x = (int) (Random.value * dimensions.x);
            var y = (int) (Random.value * dimensions.y);
            return new Vector2Int(x, y);
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