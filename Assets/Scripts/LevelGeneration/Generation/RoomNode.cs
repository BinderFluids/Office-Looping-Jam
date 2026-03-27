using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration.Generation
{
    public class RoomNode
    {
        public Room Room { get; private set; }

        // Store neighbors in a dictionary keyed by Direction
        private Dictionary<Direction, RoomNode> _neighbors = new Dictionary<Direction, RoomNode>();

        public RoomNode(Room room)
        {
            Room = room;
        }

        /// <summary>
        /// Set a neighbor in a given direction
        /// </summary>
        public void SetNeighbor(Direction direction, RoomNode neighbor)
        {
            _neighbors[direction] = neighbor;
        }

        /// <summary>
        /// Get the neighbor in a given direction, or null if none
        /// </summary>
        public RoomNode GetNeighbor(Direction direction)
        {
            _neighbors.TryGetValue(direction, out var neighbor);
            return neighbor;
        }

        /// <summary>
        /// Enumerate all neighbors
        /// </summary>
        public IEnumerable<KeyValuePair<Direction, RoomNode>> GetAllNeighbors()
        {
            foreach (var kvp in _neighbors)
                yield return kvp;
        }

        /// <summary>
        /// Check if this node has a neighbor in the given direction
        /// </summary>
        public bool HasNeighbor(Direction direction)
        {
            return _neighbors.ContainsKey(direction);
        }
    }
}