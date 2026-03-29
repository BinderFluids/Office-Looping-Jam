using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelGeneration.Generation
{
    public enum TileType
    {
        Walkable = 0,
        Wall = 1,
        Inaccessible = 99
    }

    /**
     * Purpose of the layout generator is to generate the rooms and walls
     */
    public class LayoutGenerator : MonoBehaviour
    {
        [SerializeField] private int maxRooms;
        [SerializeField] private int minRooms;
        [SerializeField] private int minHallwayLength = 4;
        [SerializeField] private int maxHallwayLength = 9;
        [SerializeField] private int hallwayThickness = 5;
        [Tooltip("Chance the gap between rooms is a wide combined floor (full span) instead of a narrow hallway.")]
        [SerializeField] [Range(0f, 1f)] private float combineRoomsChance = 0.35f;

        private List<RoomNode> _roomNodes = new List<RoomNode>();
        private List<RectInt> _hallwayRects = new List<RectInt>();
        
        private RoomNode FurthestNodeInDirection(Direction direction, RoomNode beginNode)
        {
            RoomNode currentNode = beginNode;
            while (currentNode.GetNeighbor(direction) != null)
            {
                currentNode = currentNode.GetNeighbor(direction);
            }

            return currentNode;
        }

        public int[,] RoomsToTileArray()
        {
            if (_roomNodes.Count == 0)
                return null;

            var rects = new List<RectInt>();
            foreach (var node in _roomNodes)
            {
                foreach (var rect in node.Room.bounds)
                    rects.Add(rect);
            }

            foreach (var rect in _hallwayRects)
                rects.Add(rect);

            GetInclusiveBounds(rects, out int minX, out int minY, out int maxX, out int maxY);
            int width = maxX - minX + 1;
            int height = maxY - minY + 1;

            bool[,] floorMask = new bool[width, height];
            foreach (var rect in rects)
                StampRectOntoFloorMask(floorMask, rect, minX, minY, width, height);

            return TilesFromFloorMask(floorMask);
        }

        private static void GetInclusiveBounds(List<RectInt> rects, out int minX, out int minY, out int maxX, out int maxY)
        {
            minX = int.MaxValue;
            maxX = int.MinValue;
            minY = int.MaxValue;
            maxY = int.MinValue;

            foreach (var rect in rects)
            {
                minX = Mathf.Min(minX, rect.xMin);
                maxX = Mathf.Max(maxX, rect.xMax - 1);
                minY = Mathf.Min(minY, rect.yMin);
                maxY = Mathf.Max(maxY, rect.yMax - 1);
            }
        }

        private static void StampRectOntoFloorMask(bool[,] mask, RectInt rect, int originX, int originY, int gridW, int gridH)
        {
            int x0 = Mathf.Max(0, rect.xMin - originX);
            int y0 = Mathf.Max(0, rect.yMin - originY);
            int x1 = Mathf.Min(gridW, rect.xMax - originX);
            int y1 = Mathf.Min(gridH, rect.yMax - originY);

            for (int x = x0; x < x1; x++)
            for (int y = y0; y < y1; y++)
                mask[x, y] = true;
        }

        /// <summary>
        /// Floor union: walkable inside, walls only on the outer edge (shared room/hallway edges stay open).
        /// </summary>
        private static int[,] TilesFromFloorMask(bool[,] floorMask)
        {
            int width = floorMask.GetLength(0);
            int height = floorMask.GetLength(1);
            var tiles = new int[width, height];
            int wall = (int)TileType.Wall;
            int walk = (int)TileType.Walkable;
            int none = (int)TileType.Inaccessible;

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                if (!floorMask[x, y])
                {
                    tiles[x, y] = none;
                    continue;
                }

                tiles[x, y] = TouchesNonFloorNeighbor(floorMask, x, y, width, height) ? wall : walk;
            }

            return tiles;
        }

        private static bool TouchesNonFloorNeighbor(bool[,] floorMask, int x, int y, int width, int height)
        {
            if (x == 0 || !floorMask[x - 1, y])
                return true;
            if (x == width - 1 || !floorMask[x + 1, y])
                return true;
            if (y == 0 || !floorMask[x, y - 1])
                return true;
            if (y == height - 1 || !floorMask[x, y + 1])
                return true;
            return false;
        }

        private static void ComputeHallwayAndNewRoom(
            Room anchorRoom,
            Direction direction,
            int hallwayLength,
            int hallwayThickness,
            bool combineRooms,
            Vector2Int roomDims,
            out RectInt hallwayRect,
            out RectInt newRoomRect)
        {
            RectInt anchor = anchorRoom.GetCombinedBounds();
            int minX = anchor.xMin;
            int maxX = anchor.xMax;
            int minY = anchor.yMin;
            int maxY = anchor.yMax;
            int cx = (minX + maxX) / 2;
            int cy = (minY + maxY) / 2;
            int w = roomDims.x;
            int h = roomDims.y;
            int L = hallwayLength;
            int t = hallwayThickness;

            switch (direction)
            {
                case Direction.Right:
                    newRoomRect = new RectInt(maxX + L, cy - h / 2, w, h);
                    hallwayRect = combineRooms
                        ? BridgeHorizontalGap(anchor, newRoomRect, maxX, L)
                        : NarrowHallwayHorizontal(maxX, cy, L, t);
                    break;
                case Direction.Left:
                    newRoomRect = new RectInt(minX - L - w, cy - h / 2, w, h);
                    hallwayRect = combineRooms
                        ? BridgeHorizontalGap(anchor, newRoomRect, minX - L, L)
                        : NarrowHallwayHorizontal(minX - L, cy, L, t);
                    break;
                case Direction.Top:
                    newRoomRect = new RectInt(cx - w / 2, maxY + L, w, h);
                    hallwayRect = combineRooms
                        ? BridgeVerticalGap(anchor, newRoomRect, maxY, L)
                        : NarrowHallwayVertical(cx, maxY, L, t);
                    break;
                case Direction.Bottom:
                    newRoomRect = new RectInt(cx - w / 2, minY - L - h, w, h);
                    hallwayRect = combineRooms
                        ? BridgeVerticalGap(anchor, newRoomRect, minY - L, L)
                        : NarrowHallwayVertical(cx, minY - L, L, t);
                    break;
                default:
                    hallwayRect = default;
                    newRoomRect = default;
                    break;
            }
        }

        private static RectInt NarrowHallwayHorizontal(int xMin, int centerY, int length, int thickness)
        {
            return new RectInt(xMin, centerY - thickness / 2, length, thickness);
        }

        private static RectInt NarrowHallwayVertical(int centerX, int yMin, int length, int thickness)
        {
            return new RectInt(centerX - thickness / 2, yMin, thickness, length);
        }

        private static RectInt BridgeHorizontalGap(RectInt anchor, RectInt other, int gapXMin, int gapWidth)
        {
            int y0 = Mathf.Min(anchor.yMin, other.yMin);
            int y1 = Mathf.Max(anchor.yMax, other.yMax);
            return new RectInt(gapXMin, y0, gapWidth, y1 - y0);
        }

        private static RectInt BridgeVerticalGap(RectInt anchor, RectInt other, int gapYMin, int gapHeight)
        {
            int x0 = Mathf.Min(anchor.xMin, other.xMin);
            int x1 = Mathf.Max(anchor.xMax, other.xMax);
            return new RectInt(x0, gapYMin, x1 - x0, gapHeight);
        }
        public void GenerateRooms()
        {
            _roomNodes.Clear();
            _hallwayRects.Clear();

            RoomSize size = (RoomSize)Random.Range(0, System.Enum.GetValues(typeof(RoomSize)).Length);
            Room baseRoom = RoomGenerator.GenerateRoom(RoomGenerator.GetSettings(size), 0, 0);
            RoomNode currentNode = new RoomNode(baseRoom);
            RoomNode previousNode = currentNode;
            _roomNodes.Add(currentNode);

            int hallMin = Mathf.Min(minHallwayLength, maxHallwayLength);
            int hallMax = Mathf.Max(minHallwayLength, maxHallwayLength);
            int thickness = Mathf.Max(3, hallwayThickness | 1);

            for (int i = 0; i < maxRooms; i++)
            {
                Direction direction = (Direction)Random.Range(0, System.Enum.GetValues(typeof(Direction)).Length);
                RoomSettings roomSettings = RoomGenerator.GetSettings(size);
                currentNode = FurthestNodeInDirection(direction, currentNode);

                int hallwayLength = Random.Range(hallMin, hallMax + 1);
                Vector2Int dims = RoomGenerator.SampleDimensions(roomSettings);
                bool combineRooms = Random.value < combineRoomsChance;
                ComputeHallwayAndNewRoom(
                    currentNode.Room,
                    direction,
                    hallwayLength,
                    thickness,
                    combineRooms,
                    dims,
                    out RectInt hallwayRect,
                    out RectInt newRoomRect);

                _hallwayRects.Add(hallwayRect);

                Room room = RoomGenerator.CreateRoomFromRect(roomSettings, newRoomRect);

                previousNode = currentNode;
                currentNode = new RoomNode(room);
                _roomNodes.Add(currentNode);
                currentNode.SetNeighbor(DirectionUtils.Opposite(direction), previousNode);
                previousNode.SetNeighbor(direction, currentNode);
            }
        }
    }
}