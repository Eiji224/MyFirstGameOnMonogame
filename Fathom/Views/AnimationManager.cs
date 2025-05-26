using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class AnimationManager
{
    private readonly int _startFrame;
    private readonly int _endFrame;
    
    private int _activeFrame;
    private float _frameTime;
    private readonly float _frameDelay;
    
    private readonly Dictionary<int, Rectangle> _frames;

    public AnimationManager(int startFrame, int endFrame, Point frameSize, float frameDelay)
    {
        _startFrame = startFrame;
        _endFrame = endFrame;
        _activeFrame = startFrame;
        _frameDelay = frameDelay;
        
        _frames = new Dictionary<int, Rectangle>();
        for (var i = startFrame; i <= endFrame; i++)
            _frames.Add(i, new Rectangle(i * frameSize.X, 0, frameSize.X, frameSize.Y));
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
    
    public void ResetAnimationFrame()
    {
        _activeFrame = _startFrame;
        _frameTime = 0;
    }

    public Rectangle GetFrameCoordinates() => _frames[_activeFrame];
}