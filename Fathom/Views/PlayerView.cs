using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class PlayerView(LevelModel level) : IView
{
    private readonly Player _player = level.Player;
    private Texture2D _playerTexture;
    
    private AnimationManager _currentAnimation;
    private AnimationManager _runAnimation;
    private AnimationManager _jumpAnimation;
    private AnimationManager _idleAnimation;
    
    private readonly Point _playerDimensions = new(32, 32);
    
    public void LoadContent(ContentManager content)
    {
        var startEndMoveAnimation = (0, 3);
        var startEndJumpAnimation = (7, 9);
        var startEndIdleAnimation = (0, 0);
        const float moveAnimationSpeed = 1 / 8f;
        
        _playerTexture = content.Load<Texture2D>("player_1");
        
        _runAnimation = new AnimationManager(startEndMoveAnimation.Item1, startEndMoveAnimation.Item2,
            _playerDimensions, moveAnimationSpeed);
        _jumpAnimation = new AnimationManager(startEndJumpAnimation.Item1, startEndJumpAnimation.Item2,
            _playerDimensions, moveAnimationSpeed);
        _idleAnimation = new AnimationManager(startEndIdleAnimation.Item1, startEndIdleAnimation.Item2,
            _playerDimensions, moveAnimationSpeed);
        
        _currentAnimation = _idleAnimation;
    }

    public void Update(GameTime gameTime)
    {
        var previousAnimation = _currentAnimation;
        
        if (!_player.IsOnGround)
        {
            _currentAnimation = _jumpAnimation;
            _jumpAnimation.UpdateAnimationFrame(gameTime);
        }
        else if (_player.Velocity.X != 0)
        {
            _currentAnimation = _runAnimation;
            _runAnimation.UpdateAnimationFrame(gameTime);
        }
        else
        {
            _currentAnimation = _idleAnimation;
        }
        
        if (previousAnimation != _currentAnimation)
        {
            previousAnimation?.ResetAnimationFrame();
        }
    }

    public void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        var isFlipped = _player.Direction.X < 0;
        
        var sourceRectangle = _currentAnimation?.GetFrameCoordinates() ?? new Rectangle(0, 0, _playerDimensions.X, _playerDimensions.Y);
        spriteBatch.Draw(
            _playerTexture,
            new Rectangle((int)_player.Position.X, (int)_player.Position.Y, _playerDimensions.X, _playerDimensions.Y),
            sourceRectangle,
            Color.White,
            0f,
            Vector2.Zero,
            isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
            0f
        );
    }
}