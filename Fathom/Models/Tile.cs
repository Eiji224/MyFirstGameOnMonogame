namespace Fathom;

public enum TileType
{
    Empty,
    Ground,
    Platform,
    Spike,
    Goal
}

public class Tile(int x, int y, TileType? tileType = null)
{
    public TileType Type { get; set; } = tileType ?? TileType.Empty;
    public int X { get; } = x;
    public int Y { get; } = y;
    
    public static bool operator == (Tile tile1, TileType tileType) => tile1 != null && tile1.Type == tileType;
    public static bool operator != (Tile tile1, TileType tileType) => tile1 != null && tile1.Type != tileType;
}