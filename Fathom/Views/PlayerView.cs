using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class PlayerView(LevelModel level) : IView
{
    private readonly Player _player = level.Player;
    private Texture2D _playerTexture;
    private AnimationManager _runAnimation;
    
    private readonly Point _playerDimensions = new (32, 32);
    
    public void LoadContent(ContentManager content)
    {
        var startEndMoveAnimation = (0, 3);
        const float moveAnimationSpeed = 1 / 8f;
        
        _playerTexture = content.Load<Texture2D>("player_1");
        _runAnimation = new AnimationManager(startEndMoveAnimation.Item1, startEndMoveAnimation.Item2,
            _playerDimensions, moveAnimationSpeed);
    }

    public void Update(GameTime gameTime)
    {
        if(_player.Velocity.X != 0)
            _runAnimation.UpdateAnimationFrame(gameTime);
        else
            _runAnimation.ResetAnimationFrame();
    }

    public void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        var isFlipped = _player.Direction.X < 0;
        
        spriteBatch.Draw(
            _playerTexture,
            new Rectangle((int)_player.Position.X, (int)_player.Position.Y, _playerDimensions.X, _playerDimensions.Y),
            _runAnimation.GetFrameCoordinates(),
            Color.White,
            0f,
            Vector2.Zero,
            isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
            0f
        );
    }
}