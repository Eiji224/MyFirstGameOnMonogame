using Microsoft.Xna.Framework;

namespace Fathom;

public class TileMap
{
    private readonly Tile[,] _tiles;
    public int Width { get; }
    public int Height { get; }
    public const int TileSize = 32;

    public TileMap(int width = 400, int height = 230) // width = 400; height = 230      80 74
    {
        Width = width;
        Height = height;
        _tiles = InitializeMap(Width, Height);
    }

    public static Tile[,] InitializeMap(int width, int height)
    {
        var map = new Tile[width, height];
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                map[x, y] = new Tile(x, y);
            }
        }

        return map;
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return new Tile(x, y);
        
        return _tiles[x, y];
    }

    public void SetTile(Tile tile)
    {
        if (tile.X < 0 || tile.Y < 0 || tile.X >= Width || tile.Y >= Height)
            return;
        
        _tiles[tile.X, tile.Y] = tile;
    }

    public bool IsWalkable(int x, int y)
    {
        var tile = GetTile(x, y);
        return tile == TileType.Empty;
    }
}