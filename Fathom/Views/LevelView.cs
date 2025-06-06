using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class LevelView : IView
{
    private readonly LevelModel _level;
    private readonly TileView _tileView;
    private Texture2D _pixelTexture;
    
    public LevelView(LevelModel level)
    {
        _level = level;
        _tileView = new TileView(level.TileMap);
    }
    
    public void LoadContent(ContentManager content)
    {
        _tileView.LoadContent(content);
    }
    
    public void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        DrawBackground(spriteBatch, camera);
        _tileView.Draw(spriteBatch, camera);
    }
    
    private void DrawBackground(SpriteBatch spriteBatch, Camera camera)
    {
        var viewportBounds = new Rectangle(
            (int)camera.Position.X,
            (int)camera.Position.Y,
            (int)camera.ViewportSize.X,
            (int)camera.ViewportSize.Y
        );
        
        spriteBatch.Draw(
            GetPixelTexture(spriteBatch.GraphicsDevice),
            viewportBounds,
            null,
            new Color(135, 206, 235) // голубой цвет для неба
        );
    }
    
    private Texture2D GetPixelTexture(GraphicsDevice graphicsDevice)
    {
        if (_pixelTexture == null)
        {
            _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            _pixelTexture.SetData([Color.White]);
        }
        return _pixelTexture;
    }
}