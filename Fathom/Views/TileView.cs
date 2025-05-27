using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class TileView :IView
{
    private readonly TileMap _tileMap;
    private Texture2D _groundTexture;
    private Texture2D _platformTexture;
    
    public TileView(TileMap tileMap)
    {
        _tileMap = tileMap;
    }

    public void LoadContent(ContentManager content)
    {
        _groundTexture = content.Load<Texture2D>("tiles/ground");
        _platformTexture = content.Load<Texture2D>("sandbrick_grass_platform");
    }

    public void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        var viewportBounds = new Rectangle(
            (int)camera.Position.X / TileMap.TileSize,
            (int)camera.Position.Y / TileMap.TileSize,
            (int)camera.ViewportSize.X / TileMap.TileSize + 2,
            (int)camera.ViewportSize.Y / TileMap.TileSize + 2
        );
        
        for (var y = viewportBounds.Y; y < viewportBounds.Y + viewportBounds.Height; y++)
        {
            for (var x = viewportBounds.X; x < viewportBounds.X + viewportBounds.Width; x++)
            {
                if (x < 0 || y < 0 || x >= _tileMap.Width || y >= _tileMap.Height)
                    continue;

                var tile = _tileMap.GetTile(x, y);
                if (tile == TileType.Empty)
                    continue;

                var destinationRect = new Rectangle(
                    x * TileMap.TileSize,
                    y * TileMap.TileSize,
                    TileMap.TileSize,
                    TileMap.TileSize
                );
                
                var texture = tile.Type switch
                {
                    TileType.Ground => _groundTexture,
                    TileType.Platform => _platformTexture,
                    _ => _groundTexture
                };

                spriteBatch.Draw(texture, destinationRect, Color.White);
            }
        }
    }
} 