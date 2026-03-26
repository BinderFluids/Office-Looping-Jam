using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration
{
    [CreateAssetMenu(menuName = "Procedural/Structure Layout")]
    public class StructureLayout : ScriptableObject
    {
        public string structureName;

        [Header("Structure Tiles")]
        

        [Header("Placement Rules")]
        public StructureRule placementRules;
    }
}