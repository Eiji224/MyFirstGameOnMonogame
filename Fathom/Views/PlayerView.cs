using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class PlayerView(LevelModel level) : IView
{
    private readonly Player _player = level.Player;
    private Texture2D _playerTexture;
    private AnimationManager _runAnimation;
    
    public void LoadContent(ContentManager content)
    {
        var startEndMoveAnimation = (0, 3);
        const float moveAnimationSpeed = 1 / 8f;
        
        _playerTexture = content.Load<Texture2D>("player_1");
        _runAnimation = new AnimationManager(_playerTexture,
            startEndMoveAnimation.Item1, startEndMoveAnimation.Item2,
            new Point(32, 32), moveAnimationSpeed);
    }

    public void Update(GameTime gameTime)
    {
        if(_player.Velocity.X != 0)
            _runAnimation.UpdateAnimationFrame(gameTime);
        else
            _runAnimation.ResetAnimation();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var isFlipped = _player.Direction.X < 0;
        
        _runAnimation.DrawAnimation(spriteBatch, _player.Position, isFlipped);
    }
}