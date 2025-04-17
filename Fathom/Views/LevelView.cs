using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class LevelView(LevelModel level) : IView
{
    private readonly LevelModel _level = level;
    private Texture2D _platformTexture;

    public void LoadContent(ContentManager content)
    {
        _platformTexture = content.Load<Texture2D>("sandbrick_grass_platform");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var platform in _level.Platforms)
        {
            spriteBatch.Draw(_platformTexture, platform.Position, Color.White);
        }
    }
}