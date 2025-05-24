using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fathom.Controllers;

public class PlayerController(Player player, LevelModel levelModel)
{
    private readonly Player _player = player;
    private readonly LevelModel _levelModel = levelModel;
    
    private float _jumpTimer = 0f;
    private bool _isJumping = false;
    private const float JumpDuration = 0.3f;

    public void Update(GameTime gameTime)
    {
        HandleCollisions();
        HandleInput();
        ApplyGravity(gameTime);
        
        _player.Move(_player.Velocity);
        _player.UpdateBoundingBox();
    }

    private void HandleInput()
    {
        var keyboard = Keyboard.GetState();
        
        if (!keyboard.IsKeyDown(Keys.A) && !keyboard.IsKeyDown(Keys.D))
        {
            _player.Velocity = new Vector2(0, _player.Velocity.Y);
        }
        
        if (keyboard.IsKeyDown(Keys.A))
        {
            _player.Velocity = new Vector2(-2.5f, _player.Velocity.Y);
        }
        if (keyboard.IsKeyDown(Keys.D))
        {
            _player.Velocity = new Vector2(2.5f, _player.Velocity.Y);
        }
        if (keyboard.IsKeyDown(Keys.S))
        {
            _player.Velocity = new Vector2(_player.Velocity.X, 2.5f);
        }
        if ((keyboard.IsKeyDown(Keys.Space) || keyboard.IsKeyDown(Keys.W)) && _player.IsOnGround)
        {
            _player.Velocity = new Vector2(_player.Velocity.X, -4f);
            _player.IsOnGround = false;
            _isJumping = true;
            _jumpTimer = 0f;
        }
    }

    private void ApplyGravity(GameTime gameTime)
    {
        if (_isJumping)
        {
            _jumpTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_jumpTimer >= JumpDuration)
            {
                _isJumping = false;
            }
            return;
        }
        
        const float gravityAcceleration = 1f;
        _player.Velocity += new Vector2(0, gravityAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds * 60f);
    }

    private void HandleCollisions()
    {
        _player.IsOnGround = false;

        foreach (var platform in _levelModel.Platforms.Where(platform => _player.BoundingBox.Intersects(platform.BoundingBox)))
        {
            _player.SetPosition(new Vector2(_player.Position.X, platform.Position.Y - _player.BoundingBox.Height));
            _player.Velocity = new Vector2(_player.Velocity.X, 0);
            _player.IsOnGround = true;
            _isJumping = false;
            _jumpTimer = 0f;
        }
    }
}