/*using Microsoft.Xna.Framework;

namespace Fathom;

public enum TileType
{
    Empty,
    Ground,
    Platform,
    Spike,
    Checkpoint
}

public class Tile
{
    public TileType Type { get; }
    public bool IsCollidable { get; }
    public Rectangle BoundingBox { get; private set; }
    
    private const int TileSize = 32; // Размер тайла в пикселях
    
    public Tile(TileType type, Point position)
    {
        Type = type;
        IsCollidable = type is TileType.Ground or TileType.Platform;
        BoundingBox = new Rectangle(position.X * TileSize, position.Y * TileSize, TileSize, TileSize);
    }
    
    public void UpdatePosition(Point position)
    {
        BoundingBox = new Rectangle(position.X * TileSize, position.Y * TileSize, TileSize, TileSize);
    }
    
    public static Tile CreateTile(TileType type, Point position)
    {
        return new Tile(type, position);
    }
} */