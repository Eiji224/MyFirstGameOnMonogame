using Microsoft.Xna.Framework;

namespace Fathom;

public class TileMap
{
    private readonly int[,] _tiles;
    public int Width { get; }
    public int Height { get; }
    public const int TileSize = 32;

    public TileMap(int width = 400, int height = 230)
    {
        Width = width;
        Height = height;
        _tiles = new int[width, height];
    }

    public int GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return 0;
        
        return _tiles[x, y];
    }

    public void SetTile(int x, int y, int tileId)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return;
        
        _tiles[x, y] = tileId;
    }

    public bool IsWalkable(int x, int y)
    {
        var tile = GetTile(x, y);
        return tile == 0;
    }
}