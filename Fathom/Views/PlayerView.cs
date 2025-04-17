using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class PlayerView(LevelModel level) : IView
{
    private readonly Player _player = level.Player;
    
    private Texture2D _playerTexture;
    
    public void LoadContent(ContentManager content)
    {
        _playerTexture = content.Load<Texture2D>("player_1");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_playerTexture, _player.Position, Color.White);
    }
}