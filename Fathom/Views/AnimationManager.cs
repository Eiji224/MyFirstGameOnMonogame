using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class AnimationManager
{
    private readonly Texture2D _spriteSheet;
    private readonly int _startFrame;
    private readonly int _endFrame;
    private readonly Point _frameSize;
    
    private int _activeFrame;
    private float _frameTime;
    private float _frameDelay;

    public AnimationManager(Texture2D spriteSheet, int startFrame, int endFrame, Point frameSize, float frameDelay)
    {
        _spriteSheet = spriteSheet;
        _startFrame = startFrame;
        _endFrame = endFrame;
        _frameSize = frameSize;
        _activeFrame = startFrame;
        _frameDelay = frameDelay;
    }

    public void UpdateAnimationFrame(GameTime gameTime)
    {
        _frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_frameTime >= _frameDelay)
        {
            _frameTime = 0;
            _activeFrame++;
            
            if (_activeFrame > _endFrame)
                _activeFrame = _startFrame;
        }
    }

    public void DrawAnimation(SpriteBatch spriteBatch, Vector2 position, bool isFlipped = false)
    {
        var flipEffect = isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        
        spriteBatch.Draw(
            _spriteSheet,
            new Rectangle((int)position.X, (int)position.Y, _frameSize.X, _frameSize.Y),
            new Rectangle(_activeFrame * _frameSize.X, 0, _frameSize.X, _frameSize.Y),
            Color.White,
            0f,
            Vector2.Zero,
            flipEffect,
            0f
        );
    }

    public void ResetAnimation()
    {
        _activeFrame = _startFrame;
        _frameTime = 0;
    }
}