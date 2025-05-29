using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class LevelModel
{
    public TileMap TileMap { get; private set; }
    public Player Player { get; private set; }
    public Vector2 LevelSize => new(TileMap.Width * TileMap.TileSize, TileMap.Height * TileMap.TileSize);
    
    public LevelGenerator _levelGenerator { get; private set; } //УБРАТЬ ПОТОМ!!!

    public void Initialize()
    {
        TileMap = new TileMap();
        _levelGenerator = new LevelGenerator(null,  TileMap.Width, TileMap.Height);
        _levelGenerator.GenerateLevel(TileMap);
        
        Player = new Player(new Vector2(4 * TileMap.TileSize, (TileMap.Height - 4) * TileMap.TileSize), 25, 25);
    }

    public bool IsCollision(Rectangle boundingBox)
    {
        var startX = boundingBox.Left / TileMap.TileSize;
        var startY = boundingBox.Top / TileMap.TileSize;
        var endX = (boundingBox.Right - 1) / TileMap.TileSize;
        var endY = (boundingBox.Bottom - 1) / TileMap.TileSize;
        
        for (var y = startY; y <= endY; y++)
        {
            for (var x = startX; x <= endX; x++)
            {
                if (x < 0 || y < 0 || x >= TileMap.Width || y >= TileMap.Height)
                    return true;

                if (TileMap.GetTile(x, y) == 0) continue;
                
                var tileRect = new Rectangle(
                    x * TileMap.TileSize,
                    y * TileMap.TileSize,
                    TileMap.TileSize,
                    TileMap.TileSize
                );
                if (boundingBox.Intersects(tileRect))
                    return true;
            }
        }

        return false;
    }
}