using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration.Generation
{
    public struct RoomSettings
    {
        public int numRects;
        public int minWidth;
        public int minHeight;
        public int maxWidth;
        public int maxHeight;

        public RoomSettings(int numRects, int minW, int minH, int maxW, int maxH)
        {
            this.numRects = numRects;
            this.minWidth = minW;
            this.minHeight = minH;
            this.maxWidth = maxW;
            this.maxHeight = maxH;
        }
    }
    
    public static class RoomGenerator
    {
        public static RoomSettings GetSettings(RoomSize size)
        {
            return size switch
            {
                RoomSize.Small => new RoomSettings(0, 4, 4, 8, 8),
                RoomSize.Medium => new RoomSettings(0, 5, 5, 11, 11),
                RoomSize.Large => new RoomSettings(0, 6, 6, 13, 13),
                RoomSize.ExtraLarge => new RoomSettings(0, 8, 8, 17, 17),
                _ => new RoomSettings(0, 4, 4, 8, 8)
            };
        }

        public static Vector2Int SampleDimensions(RoomSettings settings)
        {
            return new Vector2Int(
                Random.Range(settings.minWidth, settings.maxWidth),
                Random.Range(settings.minHeight, settings.maxHeight));
        }

        public static Room CreateRoomFromRect(RoomSettings settings, RectInt rect)
        {
            return new Room(settings, new List<RectInt> { rect });
        }

        public static Room GenerateRoom(RoomSettings settings, int startX, int startY)
        {
            List<RectInt> rects = new List<RectInt>();
            RectInt baseRect = GenerateRectangle(settings, startX, startY);
            rects.Add(baseRect);
            
            for (int i = 0; i < settings.numRects; i++)
            {
                RectInt attachTo = rects[Random.Range(0, rects.Count)];
                RectInt newRect = GenerateAttachedRectangle(settings, attachTo);
                rects.Add(newRect);
            }
            
            return new Room(settings, rects);;
        }

        private static RectInt GenerateRectangle(RoomSettings settings, int x, int y)
        {
            int width = Random.Range(settings.minWidth, settings.maxWidth);
            int height = Random.Range(settings.minHeight, settings.maxHeight);
            
            return new RectInt(x, y, width, height);
        }
        
        private static RectInt GenerateAttachedRectangle(RoomSettings settings, RectInt attachTo)
        {
            int direction = Random.Range(0, 4); // 0=top, 1=right, 2=bottom, 3=left
            int width = Random.Range(settings.minWidth, settings.maxWidth);
            int height = Random.Range(settings.minHeight, settings.maxHeight);

            int x, y;

            switch (direction)
            {
                case 0:
                    x = attachTo.x + attachTo.width / 2 - width / 2;
                    y = attachTo.y + attachTo.height;
                    break;
                case 1:
                    x = attachTo.x + attachTo.width;
                    y = attachTo.y + attachTo.height / 2 - height / 2;
                    break;
                case 2:
                    x = attachTo.x + attachTo.width / 2 - width / 2;
                    y = attachTo.y - height;
                    break;
                case 3:
                    x = attachTo.x - width;
                    y = attachTo.y + attachTo.height / 2 - height / 2;
                    break;
                default:
                    x = attachTo.x;
                    y = attachTo.y;
                    break;
            }

            return new RectInt(x, y, width, height);
        }
    }
    
    
}