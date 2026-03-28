using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration.Generation
{
    public enum Direction
    {
        Left,
        Right,
        Top,
        Bottom
    }
    
    public static class Direction2D
    {
        public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
        {
            new Vector2Int(0,1), //UP
            new Vector2Int(1,0), //RIGHT
            new Vector2Int(0, -1), // DOWN
            new Vector2Int(-1, 0) //LEFT
        };

        public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
        {
            new Vector2Int(1,1), //UP-RIGHT
            new Vector2Int(1,-1), //RIGHT-DOWN
            new Vector2Int(-1, -1), // DOWN-LEFT
            new Vector2Int(-1, 1) //LEFT-UP
        };

        public static List<Vector2Int> eightDirectionsList = new List<Vector2Int>
        {
            new Vector2Int(0,1), //UP
            new Vector2Int(1,1), //UP-RIGHT
            new Vector2Int(1,0), //RIGHT
            new Vector2Int(1,-1), //RIGHT-DOWN
            new Vector2Int(0, -1), // DOWN
            new Vector2Int(-1, -1), // DOWN-LEFT
            new Vector2Int(-1, 0), //LEFT
            new Vector2Int(-1, 1) //LEFT-UP

        };

        public static Vector2Int GetRandomCardinalDirection()
        {
            return cardinalDirectionsList[UnityEngine.Random.Range(0, cardinalDirectionsList.Count)];
        }
    }

    public static class DirectionUtils
    {
        public static Direction Opposite(Direction dir)
        {
            return dir switch
            {
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                Direction.Top => Direction.Bottom,
                Direction.Bottom => Direction.Top,
                _ => dir
            };
        }
    }
}