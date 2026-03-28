using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration.Generation
{
    public class Room
    {
        public RoomSettings roomSettings;
        public List<RectInt> bounds = new List<RectInt>();

        public Room(RoomSettings settings, List<RectInt> rects)
        {
            roomSettings = settings;
            bounds = rects;
        }

        // Optional utility: add a rect
        public void AddRect(RectInt rect)
        {
            bounds.Add(rect);
        }

        public RectInt GetCombinedBounds()
        {
            if (bounds == null || bounds.Count == 0)
                return new RectInt(0, 0, 0, 0);

            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;

            foreach (var rect in bounds)
            {
                minX = Mathf.Min(minX, rect.xMin);
                maxX = Mathf.Max(maxX, rect.xMax);
                minY = Mathf.Min(minY, rect.yMin);
                maxY = Mathf.Max(maxY, rect.yMax);
            }

            return new RectInt(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Returns the center-most point along the given edge direction
        /// </summary>
        public Vector2Int GetEdgeCenter(Direction direction)
        {
            if (bounds == null || bounds.Count == 0)
                return Vector2Int.zero;

            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;

            foreach (var rect in bounds)
            {
                minX = Mathf.Min(minX, rect.xMin);
                maxX = Mathf.Max(maxX, rect.xMax);
                minY = Mathf.Min(minY, rect.yMin);
                maxY = Mathf.Max(maxY, rect.yMax);
            }

            switch (direction)
            {
                case Direction.Left:
                    return new Vector2Int(minX, (minY + maxY) / 2);
                case Direction.Right:
                    return new Vector2Int(maxX - 1, (minY + maxY) / 2);
                case Direction.Top:
                    return new Vector2Int((minX + maxX) / 2, maxY - 1);
                case Direction.Bottom:
                    return new Vector2Int((minX + maxX) / 2, minY);
                default:
                    return new Vector2Int((minX + maxX) / 2, (minY + maxY) / 2);
            }
        }

        /// <summary>
        /// Returns the geometric center of the room
        /// </summary>
        public Vector2Int GetCenter()
        {
            if (bounds.Count == 0) return Vector2Int.zero;
            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;

            foreach (var rect in bounds)
            {
                minX = Mathf.Min(minX, rect.xMin);
                maxX = Mathf.Max(maxX, rect.xMax);
                minY = Mathf.Min(minY, rect.yMin);
                maxY = Mathf.Max(maxY, rect.yMax);
            }

            return new Vector2Int((minX + maxX) / 2, (minY + maxY) / 2);
        }
    }
}