using System.Collections.Generic;

namespace LevelGeneration
{
    [System.Serializable]
    public class StructureRule
    {
        public int minCount = 0;
        public int maxCount = int.MaxValue;
        public bool avoidOverlap = true;
    }
}