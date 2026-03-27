using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelGeneration.Generation
{

    
    /**
     * Purpose of the layout generator is to generate the rooms and walls
     */
    public class LayoutGenerator
    {
        [SerializeField] private int _maxRooms;
        [SerializeField] private int _minRooms;
        private List<RoomNode> _roomNodes = new List<RoomNode>();

        private RoomNode FurthestNodeInDirection(Direction direction, RoomNode beginNode)
        {
            RoomNode currentNode = beginNode;
            while (currentNode.GetNeighbor(direction) != null)
            {
                currentNode = currentNode.GetNeighbor(direction);
            }

            return currentNode;
        }
        
        /**
         * 
         */
        private void GenerateRooms()
        {
            RoomSize size = (RoomSize)Random.Range(0, System.Enum.GetValues(typeof(RoomSize)).Length);
            Room baseRoom = RoomGenerator.GenerateRoom(RoomGenerator.GetSettings(size), 0, 0);
            RoomNode currentNode = new RoomNode(baseRoom);
            RoomNode previousNode = currentNode;

            for (int i = 0; i < _maxRooms; i++)
            {
                Direction direction = (Direction)Random.Range(0, System.Enum.GetValues(typeof(Direction)).Length);
                RoomSettings roomSettings = RoomGenerator.GetSettings(size);
                currentNode = FurthestNodeInDirection(direction, currentNode);
                
                Vector2Int position = currentNode.Room.GetEdgeCenter(direction);
                Room room = RoomGenerator.GenerateRoom(roomSettings, position.x, position.y);
                
                previousNode = currentNode;
                currentNode = new RoomNode(room);
                currentNode.SetNeighbor(DirectionUtils.Opposite(direction), previousNode);
                previousNode.SetNeighbor(direction, currentNode);
            }
        }
    }
}