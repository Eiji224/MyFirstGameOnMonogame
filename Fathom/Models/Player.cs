using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class Player(Vector2 position, int width, int height)
{
    public Vector2 Position { get; set; } = position;
    public Vector2 Direction { get; set; } = new(1, 0);
    public Rectangle BoundingBox { get; private set; } = new((int)position.X, (int)position.Y, width, height);
    public Vector2 Velocity { get; set; } = Vector2.Zero;
    public bool IsOnGround { get; set; } = false;

    public void Move(Vector2 velocity)
    {
        Position += velocity;
        
        if (velocity.X != 0)
        {
            Direction = new Vector2(Math.Sign(velocity.X), 0);
        }
        
        UpdateBoundingBox();
    }

    public void ResetVelocity() => Velocity = Vector2.Zero;

    public void UpdateBoundingBox()
    {
        BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, BoundingBox.Width, BoundingBox.Height);
    }
}