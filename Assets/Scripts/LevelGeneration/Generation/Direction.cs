namespace LevelGeneration.Generation
{
    public enum Direction
    {
        Left,
        Right,
        Top,
        Bottom
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