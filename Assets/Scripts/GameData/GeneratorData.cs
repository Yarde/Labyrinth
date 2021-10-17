using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public struct GeneratorData
    {
        public Vector2Int Dimensions;
        public Dictionary<Type, ObjectiveData> Objectives;
        public int Seed;
    }
}