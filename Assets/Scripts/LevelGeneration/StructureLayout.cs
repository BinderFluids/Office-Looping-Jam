using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration
{
    [CreateAssetMenu(menuName = "Procedural/Structure Layout")]
    public class StructureLayout : ScriptableObject
    {
        public GameObject prefab;
        
        [Header("Placement Rules")]
        public StructureRule placementRules;
    }
}