using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fathom.Controllers;

public class PlayerController(Player player, LevelModel levelModel)
{
    private readonly Player _player = player;
    private readonly LevelModel _levelModel = levelModel;
    
    private const float JumpForce = -8f;
    private const float MoveSpeed = 4f;
    private const float GravityAcceleration = 15f;

    public void Update(GameTime gameTime)
    {
        var previousPosition = _player.Position;
        
        HandleInput();
        ApplyGravity(gameTime);
        
        var horizontalMovement = new Vector2(_player.Velocity.X, 0);
        _player.Move(horizontalMovement);
        _player.UpdateBoundingBox();
        
        if (_levelModel.IsCollision(_player.BoundingBox))
        {
            _player.Position = new Vector2(previousPosition.X, _player.Position.Y);
            _player.Velocity = new Vector2(0, _player.Velocity.Y);
        }
        
        var verticalMovement = new Vector2(0, _player.Velocity.Y);
        _player.Move(verticalMovement);
        _player.UpdateBoundingBox();
        
        if (_levelModel.IsCollision(_player.BoundingBox))
        {
            _player.Position = new Vector2(_player.Position.X, previousPosition.Y);
            _player.Velocity = new Vector2(_player.Velocity.X, 0);
            
            if (_player.Velocity.Y >= 0) // Если мы двигались вниз
            {
                _player.IsOnGround = true;
            }
            else if (_player.Velocity.Y < 0) // Если мы ударились головой
            {
                _player.Velocity = new Vector2(_player.Velocity.X, 0);
            }
        }
        else
        {
            _player.IsOnGround = false;
        }
        
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
            _player.Velocity = new Vector2(-MoveSpeed, _player.Velocity.Y);
            _player.Direction = new Vector2(-1, 0);
        }
        if (keyboard.IsKeyDown(Keys.D))
        {
            _player.Velocity = new Vector2(MoveSpeed, _player.Velocity.Y);
            _player.Direction = new Vector2(1, 0);
        }
        
        var isJumpKeyDown = keyboard.IsKeyDown(Keys.Space) || keyboard.IsKeyDown(Keys.W);
        if (isJumpKeyDown && _player.IsOnGround)
        {
            _player.Velocity = new Vector2(_player.Velocity.X, JumpForce);
            _player.IsOnGround = false;
        }
    }

    private void ApplyGravity(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        _player.Velocity += new Vector2(0, GravityAcceleration * deltaTime);
        
        if (_player.Velocity.Y > GravityAcceleration)
        {
            _player.Velocity = new Vector2(_player.Velocity.X, GravityAcceleration);
        }
    }
}