using System.Collections.Generic;

namespace LevelGeneration
{
    [System.Serializable]
    public class StructureRule
    {
        public int minCount = 0;          // minimum times this structure must appear
        public int maxCount = int.MaxValue; // maximum times it can appear
        public bool mustBeAdjacentToHallway = false; // e.g., hallways
        public List<StructureLayout> mustBeAdjacentToStructures; // adjacency requirements
        public bool avoidOverlap = true;   // default true
        public float placementChance = 1f; // random chance for optional structures
    }
}